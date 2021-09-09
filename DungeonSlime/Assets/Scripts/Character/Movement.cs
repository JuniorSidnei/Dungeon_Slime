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
        
        [SerializeField] private float m_speedMultiplier;
        [SerializeField] private bool m_moving;
        [SerializeField] private LevelManager m_levelManager;
        [SerializeField] private PlayerStates m_playerStates;
        
        private float m_speed;
        private Vector2Int m_currentPos;
        private Vector2Int m_finalPos;
        private Vector2Int m_currentSize;
        private Block m_farthestBlock;
        private bool m_isPositionAdjusted = false;
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
                });
            } else if(ev.Direction == Vector2.up || ev.Direction == Vector2.down) {
                transform.DOMoveY(newPos.y, m_speed).SetEase(ease).OnComplete(() => {
                    m_moving = false;
                    transform.DOMoveX(newPos.x, 0.01f).OnComplete(() => {
                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction)); 
                    });
                    m_currentPos = m_finalPos;
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

            //verificar forma semi_v
            //verificar direção cima/baixo
            //canFit pra direita
            //canFit pra esquerda
            //if direita ajusta posicao direita
            //if esquerda ajusta posicao esquerda
            if (m_playerStates.GetCurrentForm() == PlayerStates.SlimeForms.SEMI_STRETCHED_V)  {
                if (direction == Vector2.up || direction == Vector2.down) {
                    var newAdjustedPosX = adjustedPos.x -1;
                    amount = 4;
                    adjustedPos.x = newAdjustedPosX;
                    m_isPositionAdjusted = true;
                    // var isRightFree = CanFitInPositionX(new Vector2Int(newAdjustedPosXRight, adjustedPos.y),
                    //     m_currentSize, Vector2Int.right);
                    // var isLeftFree = CanFitInPositionX(adjustedPos, m_currentSize, Vector2Int.left);
                    //
                    //
                    // if (isLeftFree)
                    // {
                    //     newPos = adjustedPos.x - 1;
                    // }
                    // else if (isRightFree)
                    // {
                    //     newPos = adjustedPos.x + 2;
                    // }
                    //
                    // adjustedPos.x = newPos;

                }
            } else if (m_playerStates.GetCurrentForm() == PlayerStates.SlimeForms.SEMI_STRETCHED_H) {
                if (direction == Vector2.right || direction == Vector2.left) {
                    var newAdjustedPosY = adjustedPos.y - 1;
                    amount = 4;
                    adjustedPos.y = newAdjustedPosY;
                    m_isPositionAdjusted = true;
                }
            }
            
            for (var i = 0; i < amount; i++) {
                //var blocksPositions = new List<Vector2Int>();
                
                if (m_levelManager.GetFarthestBlock(adjustedPos, direction, 150, out Vector2Int toPosition, out Block farthestBlock)) {
                    var newDistance = Vector2.Distance(toPosition, adjustedPos);

                    //blocksPositions.Add(toPosition);
                    if (newDistance <= 1) {
                        if (!m_isPositionAdjusted) return false;
                        
                        newDistance = (float) distance;
                        toPosition = m_finalPos;
                        farthestBlock = m_farthestBlock;
                    }

                    if (newDistance <= distance) {
                        distance = newDistance;
                        m_finalPos = toPosition;
                        m_farthestBlock = farthestBlock;
                        m_speed = (float) (distance / m_speedMultiplier);
                    }
                }
                
                adjustedPos += guidingVector;
            }

            m_isPositionAdjusted = false;
            return true;
        }
        
        private void ResolveCollision(Vector2Int currentPosition, Vector2 nextDirection) {
            var newPlayerSize = m_playerStates.GetPlayerNextSize(nextDirection);
            var newPositionOnAxis = GetNewPositionOnAxis(currentPosition, nextDirection, newPlayerSize);
            
            if (nextDirection == Vector2.right || nextDirection == Vector2.left) {
                var distanceY = newPlayerSize.y - m_currentSize.y;
                var positiveY = CanFitInPositionY(newPositionOnAxis, newPlayerSize, Vector2Int.up);
                var negativeY = CanFitInPositionY(new Vector2Int(newPositionOnAxis.x, m_currentPos.y), newPlayerSize, Vector2Int.down);
                
                switch (positiveY)
                {
                    case false when negativeY:
                    {
                        if (m_currentSize.y >= 8) {
                            newPositionOnAxis.y = m_currentPos.y;
                        }
                        else {
                            newPositionOnAxis.y = m_currentPos.y - distanceY;
                        }

                        break;
                    }
                    case true when negativeY:
                    {
                        if (m_currentSize.y >= 8 || m_currentSize.y <= 1) {
                            newPositionOnAxis.y = m_currentPos.y;
                        }
                        else {
                            newPositionOnAxis.y = m_currentPos.y - 1;   
                        }

                        break;
                    }
                    case true:
                        newPositionOnAxis.y = m_currentPos.y;
                        break;
                    default:
                        newPositionOnAxis.y -= distanceY;
                        break;
                }
            } else if(nextDirection == Vector2.up || nextDirection == Vector2.down)  {
                var distanceX = newPlayerSize.x - m_currentSize.x;
                var positiveX = CanFitInPositionX(newPositionOnAxis, newPlayerSize, Vector2Int.right);
                var negativeX = CanFitInPositionX(new Vector2Int(m_currentPos.x, newPositionOnAxis.y), newPlayerSize, Vector2Int.left);
                
                switch (positiveX)
                {
                    case false when negativeX:
                    {
                        if (m_currentSize.x >= 8) {
                            newPositionOnAxis.x = m_currentPos.x;
                        }
                        else {
                            newPositionOnAxis.x = m_currentPos.x - distanceX;   
                        }

                        break;
                    }
                    case true when negativeX:
                    {
                        if (m_currentSize.x >= 8 || m_currentSize.x <= 1) {
                            newPositionOnAxis.x = m_currentPos.x;
                        }
                        else {
                            newPositionOnAxis.x = m_currentPos.x - 1;   
                        }

                        break;
                    }
                    case true:
                        newPositionOnAxis.x = m_currentPos.x;
                        break;
                    default:
                        newPositionOnAxis.x -= distanceX;
                        break;
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
            Vector2Int nextDirection) {
            //var availableBlocks = 0;
            for (var i = 0; i < newPlayerSize.x; i++) {
                if (m_levelManager.GetTotalAvailableBlockWithinDepth(newPositionOnAxis, nextDirection, 1,
                    0)) {
                    newPositionOnAxis.x += 1;
                }
                else {
                    //totalAvailableBlocks = 0;
                    return false;
                }
            }
            
            //totalAvailableBlocks = availableBlocks;
            return true;
        }

        private bool CanFitInPositionX(Vector2Int newPositionOnAxis, Vector2Int newPlayerSize,
            Vector2Int nextDirection) {
            //var availableBlocks = 0;
            for (var i = 0; i < newPlayerSize.y; i++) {
                if (m_levelManager.GetTotalAvailableBlockWithinDepth(newPositionOnAxis, nextDirection, 1,
                    1)) {
                    newPositionOnAxis.y += 1;
                }
                else {
                    //totalAvailableBlocks = 0;
                    return false;
                }
            }
            
            //totalAvailableBlocks = availableBlocks;
            return true;
        }

        private void SetPlayerPositionAndSize(Vector2Int pos, Vector2Int size) {
            m_finalPos = pos;
            m_currentSize = size;
        }
    }
}