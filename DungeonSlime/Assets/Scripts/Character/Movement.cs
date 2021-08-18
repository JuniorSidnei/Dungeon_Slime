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

namespace DungeonSlime.Character {
    public class Movement : MonoBehaviour {
        [SerializeField] private float m_speed;
        [SerializeField] private bool m_moving;
        [SerializeField] private LevelManager m_levelManager;
        [SerializeField] private PlayerStates m_playerStates;
        
        private Vector2Int m_currentPos;
        private Vector2Int m_finalPos;
        private Vector2Int m_currentSize;
        private Block m_farthestBlock;
        private bool m_isLevelFinished = false;
        public Ease ease;
       
        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMove>(OnMove);
            
            m_currentSize = m_playerStates.GetCurrentSize(m_playerStates.GetCurrentForm());
            m_currentPos = m_levelManager.GetPlayerInitialPosition();
            var position = m_levelManager.tilemap.CellToWorld((Vector3Int) m_currentPos);
            transform.position = position;

            m_isLevelFinished = false;
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
            
            transform.DOMove(newPos, m_speed).SetEase(ease).OnComplete(() => {
                m_moving = false;
                //transform.DOMoveY(newPos.y, 0.01f);
                GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
                m_currentPos = m_finalPos;
            });
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
                        m_speed = (float) (distance / 30);
                    }

                    adjustedPos += guidingVector;
                }
            }

            return true;
        }
        
        private void ResolveCollision(Vector2Int currentPosition, Vector2 nextDirection) {
            var newPlayerSize = m_playerStates.GetPlayerNextSize(nextDirection);
            var newPositionOnAxis = GetNewPositionOnAxis(currentPosition, nextDirection, newPlayerSize);
            
            if (nextDirection == Vector2.right || nextDirection == Vector2.left) {
                var positiveY = CanFitInPositionY(newPositionOnAxis, newPlayerSize, Vector2Int.up,
                    out var totalAvailableBlocksUp);
                var negativeY = CanFitInPositionY(newPositionOnAxis, newPlayerSize, Vector2Int.down,
                    out var totalAvailableBlocksDown);
                var distanceY = newPlayerSize.y - m_currentSize.y;

                if (distanceY <= 0) {
                    distanceY = 1;
                }
                
                if (!positiveY && negativeY) {
                    newPositionOnAxis.y = m_currentPos.y - distanceY;
                }
                else if (positiveY && negativeY) {
                    newPositionOnAxis.y -= distanceY - 1;
                }
                else if (!positiveY && !negativeY) {
                    newPositionOnAxis.y -= distanceY;
                }
            } else if(nextDirection == Vector2.up || nextDirection == Vector2.down)  {
                var positiveX = CanFitInPositionX(newPositionOnAxis, newPlayerSize, Vector2Int.right,
                    out var totalAvailableBlocksUp);
                var negativeX = CanFitInPositionX(newPositionOnAxis, newPlayerSize, Vector2Int.left,
                    out var totalAvailableBlocksDown);
                var distanceX = newPlayerSize.x - m_currentSize.x;
                var positionDistance = newPositionOnAxis.x - m_currentPos.x;

                if (distanceX <= 0) {
                    distanceX = 1;
                }
                
                if (!positiveX && negativeX) {
                    newPositionOnAxis.x = m_currentPos.x - distanceX;
                } else if (positiveX && negativeX) {
                    if (positionDistance > 0) {
                        newPositionOnAxis.x = m_currentPos.x;
                    }
                    else {
                        newPositionOnAxis.x -= distanceX - 1;
                    }
                } else if (positiveX) {
                    newPositionOnAxis.x = m_currentPos.x;
                }  else {
                    newPositionOnAxis.x -= distanceX;
                }
            }


            SetPlayerPositionAndSize(new Vector2Int(newPositionOnAxis.x, newPositionOnAxis.y), newPlayerSize);
        }

        private Vector2Int GetNewPositionOnAxis(Vector2Int currentFinalPos, Vector2 nextDirection, Vector2Int nextPlayerSize) {
            var newX = currentFinalPos.x;
            var newY = currentFinalPos.y;
            
            if (nextDirection == Vector2.right) {
               newX = currentFinalPos.x - nextPlayerSize.x;
            } else if (nextDirection == Vector2.left) {
                newX = currentFinalPos.x + 1;
            } else if (nextDirection == Vector2.up) {
                newY = currentFinalPos.y - nextPlayerSize.y;
            } else if (nextDirection == Vector2.down) {
                newY = currentFinalPos.y + 1;
            }
            
            return new Vector2Int(newX, newY);
        }

    
        private bool CanFitInPositionY(Vector2Int newPositionOnAxis, Vector2Int newPlayerSize,
            Vector2Int nextDirection, out int totalAvailableBlocks) {
            var availableBlocks = 0;
            for (var i = 0; i < newPlayerSize.x; i++) {
                if (m_levelManager.GetTotalAvailableBlockWithinDepth(newPositionOnAxis, nextDirection, newPlayerSize.y,
                    1, out availableBlocks)) {
                    newPositionOnAxis.x += 1;
                }
                else {
                    totalAvailableBlocks = 0;
                    return false;
                }
            }
            
            totalAvailableBlocks = availableBlocks;
            return true;
        }

        private bool CanFitInPositionX(Vector2Int newPositionOnAxis, Vector2Int newPlayerSize,
            Vector2Int nextDirection, out int totalAvailableBlocks) {
            var availableBlocks = 0;
            for (var i = 0; i < newPlayerSize.y; i++) {
                if (m_levelManager.GetTotalAvailableBlockWithinDepth(newPositionOnAxis, nextDirection, newPlayerSize.x,
                    1, out availableBlocks)) {
                    newPositionOnAxis.y += 1;
                }
                else {
                    totalAvailableBlocks = 0;
                    return false;
                }
            }
            
            totalAvailableBlocks = availableBlocks;
            return true;
        }

        private void SetPlayerPositionAndSize(Vector2Int pos, Vector2Int size) {
            m_finalPos = pos;
            m_currentSize = size;
        }
    }
}