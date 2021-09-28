using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DungeonSlime.Managers;
using DungeonSlime.Scriptables;
using DungeonSlime.Utils;
using GameToBeNamed.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonSlime.Character {
    public class Movement : MonoBehaviour {
        
        [SerializeField] private float m_speedMultiplier;
        [SerializeField] private bool m_moving;
        [SerializeField] private LevelManager m_levelManager;
        [SerializeField] private PlayerStates m_playerStates;
        
        private float m_speed;
        private Vector2Int m_currentPos;
        private Vector2Int m_finalPos;
        private Vector2Int m_deadPos;
        private Vector2Int m_currentSize;
        private Vector2Int m_basePositionOnAxis;
        private Block m_farthestBlock;
        private bool m_isDead = false;
        private int m_movementOffset = 0;
        public Ease ease;
       
        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMove>(OnMove);
            
            m_currentSize = m_playerStates.GetCurrentSize(m_playerStates.GetCurrentForm());
            m_currentPos = m_levelManager.GetPlayerInitialPosition();
            var position = m_levelManager.tilemap.CellToWorld((Vector3Int) m_currentPos);
            transform.position = position;
        }

        private void OnMove(OnMove ev) {
            if (m_moving || ev.Direction == Vector2Int.zero) return;

            m_moving = true;

            if (!GetNextPositionOnGrid(ev.Direction, m_currentPos)) {
                m_moving = false;
                return;
            }
            
            if(PlayerDied(m_finalPos, m_deadPos)) {
                m_isDead = true;
            }
            
            ResolveCollision(m_finalPos, ev.Direction);

            if (m_levelManager.IsPlayerDead()) {
                m_isDead = true;
            }
            
            var newPos = m_levelManager.tilemap.CellToLocal(new Vector3Int(m_finalPos.x, m_finalPos.y, 0));

            if (ev.Direction == Vector2.right || ev.Direction == Vector2.left) {
                transform.DOMoveX(newPos.x, m_speed).SetEase(ease).OnComplete(() => {
                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
                    
                    if (m_isDead) {
                        GameManager.Instance.LoadCurrentScene();
                    }
                    
                    transform.DOMoveY(newPos.y, 0.01f).OnComplete(() => {
                        m_moving = false;        
                    });
                    m_currentPos = m_finalPos;
                });
                
            } else if(ev.Direction == Vector2.up || ev.Direction == Vector2.down) {
                transform.DOMoveY(newPos.y, m_speed).SetEase(ease).OnComplete(() => {
                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
                    
                    if (m_isDead) {
                        GameManager.Instance.LoadCurrentScene();
                    }
                    
                    transform.DOMoveX(newPos.x, 0.01f).OnComplete(() => {
                        m_moving = false;     
                    });
                    m_currentPos = m_finalPos;
                });
            }
        }
        
        private bool PlayerDied(Vector2Int finalPos, Vector2Int deadPos) {
            return finalPos.x == deadPos.x || finalPos.y == deadPos.y;
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
                amount = m_playerStates.GetCurrentSize(m_playerStates.GetCurrentForm()).y;
            } else if (direction == Vector2.up || direction == Vector2.down) {
                guidingVector = Vector2Int.right;
                amount = m_playerStates.GetCurrentSize(m_playerStates.GetCurrentForm()).x;
            }

            for (var i = 0; i < amount; i++) {
                if (m_levelManager.GetNearestBlock(adjustedPos, direction, 150, out Vector2Int toPosition, out Vector2Int nearestSpikeIndex)) {
                    var wallDistance = Vector2.Distance(toPosition, adjustedPos);
                    
                    if (wallDistance <= 1) {
                        return false;
                    }

                    if (wallDistance < oldWallDistance) {
                        oldWallDistance = wallDistance;
                        m_finalPos = toPosition;
                        m_speed = (float) (oldWallDistance / m_speedMultiplier);
                    }

                    if (nearestSpikeIndex.x > 0 && nearestSpikeIndex.y > 0) {
                        m_deadPos = nearestSpikeIndex;
                    }
                }
                
                adjustedPos += guidingVector;
            }
            
            return true;
        }
        
        private void ResolveCollision(Vector2Int currentPosition, Vector2 nextDirection) {
            var newPlayerSize = m_playerStates.GetPlayerNextSize(nextDirection);
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
            var newX = currentFinalPos.x;
            var newY = currentFinalPos.y;
            
            var isSpecialCase = m_playerStates.GetCurrentForm() == PlayerStates.SlimeForms.FULL_STRETCHED_V;
            
            if (nextDirection == Vector2.right) {
                newX = currentFinalPos.x - nextPlayerSize.x;
                newY = FixCurrentPosition(m_currentSize, m_currentPos.y, true, nextPlayerSize);
            } else if (nextDirection == Vector2.left) {
                newX = currentFinalPos.x + 1;
                newY = FixCurrentPosition(m_currentSize, m_currentPos.y, true, nextPlayerSize);
            } else if (nextDirection == Vector2.up) {
                newY = currentFinalPos.y - nextPlayerSize.y;
                newX = FixCurrentPosition(m_currentSize, m_currentPos.x, true, nextPlayerSize);
            } else if (nextDirection == Vector2.down) {
                newY = currentFinalPos.y + 1;
                newX = FixCurrentPosition(m_currentSize, m_currentPos.x, true, nextPlayerSize);
            }
            
            return new Vector2Int(newX, newY);
        }

        private int FixCurrentPosition(Vector2Int size, int currentValue, bool isSpecialCase, Vector2Int nextSize) {
            if (size.x >= 8 || size.y >= 8) {
                if (nextSize.x >= 8 || nextSize.y >= 8) {
                    return currentValue;
                }

                return currentValue - 1;
            }
            
//            if (size.x < 8 || size.y < 8) {
//                if (nextSize.x >= 8 || nextSize.y >= 8) {
//                    return currentValue;
//                }
//
//                return currentValue - 1;
//            }
            
            if (isSpecialCase) {
                return currentValue - 1;
            }
          
            return currentValue;
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
            return true;
        }

        private void SetPlayerPositionAndSize(Vector2Int pos, Vector2Int size) {
            m_finalPos = pos;
            m_currentSize = size;
        }
    }
}