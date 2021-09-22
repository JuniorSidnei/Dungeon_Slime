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
            
            ResolveCollision(m_finalPos, ev.Direction);
            var newPos = m_levelManager.tilemap.CellToLocal(new Vector3Int(m_finalPos.x, m_finalPos.y, 0));

            if (ev.Direction == Vector2.right || ev.Direction == Vector2.left) {
                transform.DOMoveX(newPos.x, m_speed).SetEase(ease).OnComplete(() => {
                    m_moving = false;
                    transform.DOMoveY(newPos.y, 0.01f).OnComplete(() => {
                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));    
                    });
                    m_currentPos = m_finalPos;
                    
                    if (m_levelManager.IsPlayerDead()) {
                        SceneManager.LoadScene("level_07");
                    }
                });
            } else if(ev.Direction == Vector2.up || ev.Direction == Vector2.down) {
                transform.DOMoveY(newPos.y, m_speed).SetEase(ease).OnComplete(() => {
                    m_moving = false;
                    transform.DOMoveX(newPos.x, 0.01f).OnComplete(() => {
                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction)); 
                    });
                    m_currentPos = m_finalPos;
                    
                    if (m_levelManager.IsPlayerDead()) {
                        SceneManager.LoadScene("level_07");
                    }
                });
            }
        }
        
        private bool GetNextPositionOnGrid(Vector2Int direction, Vector2Int currentPos) {
            var distance = 5000.0;

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
                if (m_levelManager.GetFarthestBlock(adjustedPos, direction, 150, out Vector2Int toPosition, out Block farthestBlock)) {
                    var newDistance = Vector2.Distance(toPosition, adjustedPos);

                    if (newDistance <= 1) {
                        return false;
                    }

                    if (newDistance < distance) {
                        distance = newDistance;
                        m_finalPos = toPosition;
                        m_farthestBlock = farthestBlock;
                        m_speed = (float) (distance / m_speedMultiplier);
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
                if (CanFitInPosition(new Vector2Int(newPositionOnAxis.x, newPositionOnAxis.y),
                    newPlayerSize,
                    Vector2Int.up,
                    false, true,
                    Vector2Int.down,
                    100)) {
                    SetPlayerPositionAndSize(new Vector2Int(m_basePositionOnAxis.x, m_basePositionOnAxis.y), newPlayerSize);
                }
                else {
                    SetPlayerPositionAndSize(m_currentPos, m_currentSize);
                }
            } else if(nextDirection == Vector2.up || nextDirection == Vector2.down)  {
                if(CanFitInPosition(new Vector2Int(newPositionOnAxis.x, newPositionOnAxis.y),
                    newPlayerSize,
                    Vector2Int.right,
                    true, true,
                    Vector2Int.left, 
                    100)) {
                    SetPlayerPositionAndSize(new Vector2Int(m_basePositionOnAxis.x, m_basePositionOnAxis.y), newPlayerSize);
                }
                else {
                    SetPlayerPositionAndSize(m_currentPos, m_currentSize);
                }
            }
        }

        private Vector2Int GetNewPositionOnAxis(Vector2Int currentFinalPos, Vector2 nextDirection, Vector2Int nextPlayerSize) {
            var newX = currentFinalPos.x;
            var newY = currentFinalPos.y;
            
            var isSpecialCase = m_playerStates.GetCurrentForm() == PlayerStates.SlimeForms.FULL_STRETCHED_V;
            
            if (nextDirection == Vector2.right) {
                newX = currentFinalPos.x - nextPlayerSize.x;

                newY = FixCurrentPosition(m_currentSize, m_currentPos.y, true);
            } else if (nextDirection == Vector2.left) {
                newX = currentFinalPos.x + 1;
                newY = FixCurrentPosition(m_currentSize, m_currentPos.y, true);
            } else if (nextDirection == Vector2.up) {
                newY = currentFinalPos.y - nextPlayerSize.y;
                newX = FixCurrentPosition(m_currentSize, m_currentPos.x, isSpecialCase);
            } else if (nextDirection == Vector2.down) {
                newY = currentFinalPos.y + 1;
                newX = FixCurrentPosition(m_currentSize, m_currentPos.x, isSpecialCase);
            }
            
            return new Vector2Int(newX, newY);
        }

        private int FixCurrentPosition(Vector2Int size, int currentValue, bool isSpecialCase) {
            if (size.x < 8 && size.y < 8) return currentValue - 1;
            
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