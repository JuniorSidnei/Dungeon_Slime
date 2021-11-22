using System;
using DG.Tweening;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;
using Utils;

namespace DungeonSlime.Character {
    public class CharacterMovement : MonoBehaviour {
        
        [SerializeField] private float m_speedMultiplier;
        [SerializeField] private bool m_moving;
        [SerializeField] private bool m_willExpandShape;
        private LevelManager m_levelManager;
        [SerializeField] private CharacterStates m_characterStates;
        public Ease ease;
        private float m_speed;
        private Vector2Int m_currentPos;
        private Vector2Int m_finalPos;
        private Vector2Int m_currentSize;
        private Vector2Int m_currentNewSize;
        private Vector2Int m_basePositionOnAxis;
        private Vector2Int m_currentDirection;
        private Vector2Int m_splatterSpawnPosition;
        private bool m_isObjectDead;
        private bool m_alreadyFindPosition;
        private bool m_alreadyHasPosition;
        private bool m_isBlockCollision;
        private Sequence m_movementSequence;
        private int m_id;
        private int m_resetRockId = 0;
        private CharacterStates.CharacterType m_charType;
        

        public Vector2Int CurrentDirection {
            get => m_currentDirection;
            set => m_currentDirection = value;
        }

        public Vector2Int CurrentPosition {
            get => m_currentPos;
            set => m_currentPos = value;
        }

        public Vector2Int CurrentFinalPosition {
            get => m_finalPos;
            set => m_finalPos = value;
        }

        public Vector2Int CurrentSize {
            get => m_currentSize;
            set => m_currentSize = value;
        }

        public bool IsMoving => m_moving;

        public bool IsBlockCollision {
            get => m_isBlockCollision;
            set => m_isBlockCollision = value;
        }

        public Sequence MovementSequence {
            get => m_movementSequence;
            set => m_movementSequence = value;
        }

        private void Start() {
            m_currentSize = m_characterStates.GetCurrentSize(m_characterStates.GetCurrentForm());
            if (!m_willExpandShape) return;
            
            m_currentPos = m_levelManager.GetPlayerInitialPosition();
            var position = m_levelManager.tilemap.CellToWorld((Vector3Int) m_currentPos);
            transform.position = position;
        }

        public void SetLevelManager(LevelManager levelManager) {
            m_levelManager = levelManager;
        }
        
        public void SetInitialPosition(Vector2Int initialPositionOnGrid) {
            m_currentPos = initialPositionOnGrid;
            var position = m_levelManager.tilemap.CellToWorld((Vector3Int) m_currentPos);
            transform.position = position;
        }

        public void SetCharacterId(int id) {
            m_id = id;
        }
        
        public void OnMove(Vector2Int movementDirection, bool alreadyHasPosition, CharacterStates.CharacterType charType) {
            if (m_moving || movementDirection == Vector2Int.zero) return;

            m_currentDirection = movementDirection;
            m_alreadyHasPosition = alreadyHasPosition;
            
            
            if (!m_alreadyHasPosition) {
                if (!GetNextPositionOnGrid(m_currentDirection, m_currentPos)) {
                    m_moving = false;
                    GameManager.Instance.GlobalDispatcher.Emit(new OnRockUnableToMove(m_id));
                    return;
                }
            }
            else {
                m_isObjectDead = false;
                m_speed = 0.001f;
            }

            m_moving = true;
            m_charType = charType;
            GameManager.Instance.GlobalDispatcher.Emit(new OnRockUnableToMove(m_resetRockId));
            
            ResolveCollision(m_finalPos, m_currentDirection);
            m_alreadyFindPosition = false;
            var newPos = m_levelManager.tilemap.CellToLocal(new Vector3Int(m_finalPos.x, m_finalPos.y, 0));
            
            m_movementSequence = DOTween.Sequence();
            
            if (m_currentDirection == Vector2.right || m_currentDirection == Vector2.left) {
                m_movementSequence.Append(transform.DOMoveX(newPos.x, m_speed).SetEase(ease).OnComplete(() =>  {
                    m_moving = false;
                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_currentDirection, m_id, m_charType));
                    
                    if (m_levelManager.IsObjectDead || m_isObjectDead) {
                        if (m_charType == CharacterStates.CharacterType.Rock) {
                            m_movementSequence.Kill();
                            Destroy(gameObject);
                            m_levelManager.IsObjectDead = false;
                        }
                        else {
                            GameManager.Instance.LoadCurrentScene();
                        }
                    }

                    transform.DOMoveY(newPos.y, 0.01f).OnComplete(() => {
                        m_moving = false;
                        m_alreadyFindPosition = false;
                        m_currentSize = m_currentNewSize;

                        if (m_charType != CharacterStates.CharacterType.Slime) return;
                        
                        GameManager.Instance.GlobalDispatcher.Emit(new OnSpawnSplatter(m_currentDirection, m_characterStates.GetCurrentForm(), m_isBlockCollision));
                        m_isBlockCollision = false;
                    });
                }));
            } else if(m_currentDirection == Vector2.up || m_currentDirection == Vector2.down) {

                m_movementSequence.Append(transform.DOMoveY(newPos.y, m_speed).SetEase(ease).OnComplete(() => {
                    m_moving = false;
                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_currentDirection, m_id, m_charType));

                    if (m_levelManager.IsObjectDead || m_isObjectDead) {
                        if (m_charType == CharacterStates.CharacterType.Rock) {
                            m_movementSequence.Kill();
                            Destroy(gameObject);
                            m_levelManager.IsObjectDead = false;
                        }
                        else {
                            GameManager.Instance.LoadCurrentScene();
                        }
                    }

                    transform.DOMoveX(newPos.x, 0.01f).OnComplete(() => {
                        m_alreadyFindPosition = false;
                        m_currentSize = m_currentNewSize;
                        
                        if (m_charType != CharacterStates.CharacterType.Slime) return;
                        
                        GameManager.Instance.GlobalDispatcher.Emit(new OnSpawnSplatter(m_currentDirection, m_characterStates.GetCurrentForm(), m_isBlockCollision));
                        m_isBlockCollision = false;
                    });
                }));
            }
            
        }

        private bool GetNextPositionOnGrid(Vector2Int direction, Vector2Int currentPos) {
            var oldWallDistance = 5000.0;

            var amount = 0;
            var adjustedPos = Vector2Int.zero;
            var guidingVector = Vector2Int.zero;
            
            if (direction == Vector2Int.right) {
                adjustedPos.x = currentPos.x + m_currentSize.x - 1;
                adjustedPos.y = currentPos.y;
            } else if (direction == Vector2Int.up) {
                adjustedPos.y = currentPos.y + m_currentSize.y - 1;
                adjustedPos.x = currentPos.x;
            } else {
                adjustedPos = currentPos;
            }
            
            if (direction == Vector2Int.right || direction == Vector2Int.left) {
                guidingVector = Vector2Int.up;
                amount = m_characterStates.GetCurrentSize(m_characterStates.GetCurrentForm()).y;
            } else if (direction == Vector2.up || direction == Vector2.down) {
                guidingVector = Vector2Int.right;
                amount = m_characterStates.GetCurrentSize(m_characterStates.GetCurrentForm()).x;
            }

            for (var i = 0; i < amount; i++) {
                if (m_levelManager.GetNearestBlock(adjustedPos, direction, 150, out Vector2Int toPosition, out Block nearestBlock)) {
                    var wallDistance = Vector2.Distance(toPosition, adjustedPos);
                    
                    if (wallDistance <= 1) {
                        return false;
                    }

                    if (wallDistance < oldWallDistance) {
                        oldWallDistance = wallDistance;
                        m_finalPos = toPosition;
                        m_speed = (float) (oldWallDistance / m_speedMultiplier);

                        m_isObjectDead = nearestBlock.type == Block.BlockType.Spikes;
                    }
                }
                
                adjustedPos += guidingVector;
            }
            
            return true;
        }
        
        private void ResolveCollision(Vector2Int currentPosition, Vector2 nextDirection) {
            var newPlayerSize = m_characterStates.GetNextSize(nextDirection);
            var newPositionOnAxis = GetNewPositionOnAxis(currentPosition, nextDirection, newPlayerSize);

            if (nextDirection == Vector2.right || nextDirection == Vector2.left) {
                SetupCanFitPosition(newPositionOnAxis,
                    newPlayerSize,
                    Vector2Int.up, 
                    Vector2Int.down, 
                    false,
                    true,
                    20);
            } else if(nextDirection == Vector2.up || nextDirection == Vector2.down)  {
                SetupCanFitPosition(newPositionOnAxis,
                     newPlayerSize, 
                    Vector2Int.right,
                    Vector2Int.left,
                    true,
                    true,
                    20);
            }
        }

        private void SetupCanFitPosition(Vector2Int newPositionOnAxis, Vector2Int newPlayerSize,
            Vector2Int nextDirection, Vector2Int invertedDirection,
            bool isHorizontal, bool needUpdateBasePosition, int depth) {
            if (CanFitInPosition(newPositionOnAxis, newPlayerSize, nextDirection, isHorizontal, needUpdateBasePosition,
                invertedDirection, depth)) {
                SetPlayerPositionAndSize(new Vector2Int(m_basePositionOnAxis.x, m_basePositionOnAxis.y), newPlayerSize);
            }
            else {
                SetPlayerPositionAndSize(m_currentPos, m_currentSize);
            }
        }
        
        private Vector2Int GetNewPositionOnAxis(Vector2Int currentFinalPos, Vector2 nextDirection, Vector2Int nextPlayerSize) {
            if (m_alreadyHasPosition && m_charType == CharacterStates.CharacterType.Rock) return currentFinalPos;

            m_splatterSpawnPosition = currentFinalPos;
            var newX = currentFinalPos.x;
            var newY = currentFinalPos.y;

            if (nextDirection == Vector2.right) {
                newX = currentFinalPos.x - nextPlayerSize.x;
                newY = FixCurrentPosition(m_currentSize, m_currentPos.y, nextPlayerSize);
            } else if (nextDirection == Vector2.left) {
                newX = currentFinalPos.x + 1;
                newY = FixCurrentPosition(m_currentSize, m_currentPos.y, nextPlayerSize);
            } else if (nextDirection == Vector2.up) {
                newY = currentFinalPos.y - nextPlayerSize.y;
                newX = FixCurrentPosition(m_currentSize, m_currentPos.x, nextPlayerSize);
            } else if (nextDirection == Vector2.down) {
                newY = currentFinalPos.y + 1;
                newX = FixCurrentPosition(m_currentSize, m_currentPos.x, nextPlayerSize);
            }
            
            return new Vector2Int(newX, newY);
        }

        private int FixCurrentPosition(Vector2Int size, int currentValue, Vector2Int nextSize) {
            if (!m_willExpandShape || m_alreadyHasPosition) return currentValue;

            if (size.x < 12 && size.y < 12) {
                if (nextSize.x == 9) {
                    return currentValue - 2;
                }
                return currentValue - 1;
            }
            
            if (nextSize.x >= 12 || nextSize.y >= 12) {
                return currentValue;
            }
            
            return currentValue - 1;
        }
        
        private bool CanFitInPosition(Vector2Int newPositionOnAxis, Vector2Int newPlayerSize, Vector2Int nextDirection,
            bool isHorizontal, bool needUpdateBasePosition, Vector2Int invertedDirection, int depth) {

            if (depth == 0) {
                return false;
            }

            var axisSize = 0;
            var countSize = 0;
            Vector2Int axisMovement;

            if (needUpdateBasePosition) {
                m_basePositionOnAxis = newPositionOnAxis;
            }


            if (isHorizontal) {
                axisSize = newPlayerSize.x;
                countSize = newPlayerSize.y;
                axisMovement = Vector2Int.up;
            } else {
                axisSize = newPlayerSize.y;
                countSize = newPlayerSize.x;
                axisMovement = Vector2Int.right;
            }
            
            
            for (var i = 0; i < countSize; i++) {
                if (m_levelManager.GetTotalAvailableBlockWithinDepth(newPositionOnAxis, nextDirection, axisSize, 0, false, out var totalAvailableBlocks)) {
                    newPositionOnAxis += axisMovement;
                }
                else {

                    if (depth == 0) {
                        return true;
                    }

                    if (m_alreadyFindPosition) {
                        continue;
                    }
                    
                    if (m_charType == CharacterStates.CharacterType.Rock) {
                        m_movementSequence.Kill();
                        Destroy(gameObject);
                    }
                    
                    //if the first index is wall, change direction
                    if (totalAvailableBlocks <= 1) {
                        if (isHorizontal) {
                            newPositionOnAxis.x++;
                            m_basePositionOnAxis.x++;
                        }
                        else {
                            newPositionOnAxis.y++;
                            m_basePositionOnAxis.y++;
                        }
                        
                        //update basePos
                        newPositionOnAxis = m_basePositionOnAxis;
                        CanFitInPosition(newPositionOnAxis, newPlayerSize, nextDirection, isHorizontal, false, invertedDirection, depth - 1);   
                    }
                    else {
                        
                        if (isHorizontal) {
                            newPositionOnAxis.x--;
                            m_basePositionOnAxis.x--;
                        }
                        else {
                            newPositionOnAxis.y--;
                            m_basePositionOnAxis.y--;
                        }

                        newPositionOnAxis = m_basePositionOnAxis;
                        CanFitInPosition(newPositionOnAxis, newPlayerSize, nextDirection, isHorizontal, false, invertedDirection, depth - 1);
                    }
                }
            }

            m_alreadyFindPosition = true;
            return true;
        }

        private void SetPlayerPositionAndSize(Vector2Int pos, Vector2Int size) {
            m_finalPos = pos;
            m_currentPos = m_finalPos;
            m_currentNewSize = size;
            m_alreadyHasPosition = false;
        }
        
        public void StopMovement() {
            m_movementSequence.Kill();
            m_isBlockCollision = true;
            m_moving = false;
        }
    }
}