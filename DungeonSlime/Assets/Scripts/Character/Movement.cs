using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DungeonSlime.Managers;
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

        private Sprite m_sprite;
        private Vector2Int m_currentPos;
        private Vector2Int m_finalPos;
        private Vector2Int m_currentSize;
        private Block m_farthestBlock;
        public Ease ease;
       
        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMove>(OnMove);
            m_currentSize = m_playerStates.GetCurrentSize(m_playerStates.GetCurrentForm());
        }

        private void OnMove(OnMove ev) {
            if (m_moving) return;

            m_currentPos = (Vector2Int) m_levelManager.wallMap.WorldToCell(transform.position);
            GetNextPositionOnGrid(ev.Direction, m_currentPos);
            ResolveCollision(m_finalPos, ev.Direction);
            var newPos = m_levelManager.wallMap.CellToLocal(new Vector3Int(m_finalPos.x, m_finalPos.y, 0));
            
            m_moving = true;
            transform.DOMoveX(newPos.x, m_speed).SetEase(ease).OnComplete(() => {
                m_moving = false;
                transform.DOMoveY(newPos.y, 0.01f);
                GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
            }); 
            
            if (ev.Direction == Vector2Int.right) {
                //para direita é: -0.5f (cellSizeX / 2) = 1/2
//                transform.DOMoveX(newPos.x, m_speed).SetEase(ease).OnComplete(() => {
//                    m_moving = false;
//                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
//                    transform.DOMoveY(newPos.y, 0.1f);
//                }); 
            }//para esquerda é: +1.5f ((cellSizeX + 0.5f) = 1 + 0.5
            else if (ev.Direction == Vector2Int.left) {
//                transform.DOMoveX(newPos.x + (spriteHalfSize.x + cellSize.x), m_speed).SetEase(ease).OnComplete(() => {
//                    m_moving = false;
//                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
//                });
            } //para baixo é: + 1.5f (cellSizeY + 0.5f) = 1 + 0.5
            else if (ev.Direction == Vector2Int.down) {
//                transform.DOMoveY(newPos.y + (spriteHalfSize.y + cellSize.x), m_speed).SetEase(ease).OnComplete(() => {
//                    m_moving = false;
//                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
//                });
            } //para cima é: -0.5f cellSizeY / 2 = 1/2
            else if (ev.Direction == Vector2Int.up) {
//                transform.DOMoveY(newPos.y - spriteHalfSize.y, m_speed).SetEase(ease).OnComplete(() => {
//                    m_moving = false;
//                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
//                });
            }
        }
        
        private void GetNextPositionOnGrid(Vector2Int direction, Vector2Int currentPos) {
            var distance = 5000.0;

            var amount = m_playerStates.GetCurrentSize(m_playerStates.GetCurrentForm()).y;

            var guidingVector = Vector2Int.zero;
            if (direction == Vector2Int.right || direction == Vector2Int.left) {
                guidingVector = Vector2Int.up;
            } else if (direction == Vector2.up || direction == Vector2.down) {
                guidingVector = Vector2Int.right;
            }

            for (var i = 0; i < amount; i++) {
                if (m_levelManager.GetFarthestBlock(currentPos, direction, out Vector2Int toPosition, out Block farthestBlock)) {
                    var newDistance = Vector2.Distance(toPosition, currentPos);

                    if (newDistance < distance) {
                        distance = newDistance;
                        m_finalPos = toPosition;
                        m_farthestBlock = farthestBlock;
                        m_speed = (float) (distance / 80);
                    }

                    currentPos += guidingVector;
                }
            }
        }
        
        private void ResolveCollision(Vector2Int currentPosition, Vector2 nextDirection) {
            var newPlayerSize = m_playerStates.GetPlayerNextSize(nextDirection);
            var newPositionOnAxis = GetNewPositionOnAxis(currentPosition, nextDirection, newPlayerSize);

            //direção DIREITA E ESQUERDA
            var positiveY = CanFitInPositionY(newPositionOnAxis, newPlayerSize, Vector2Int.up, out var totalAvailableBlocksUp);
            var negativeY = CanFitInPositionY(newPositionOnAxis, newPlayerSize, Vector2Int.down, out var totalAvailableBlocksDown);
            var distanceY = newPlayerSize.y - m_currentSize.y;
            
            if (!positiveY && negativeY) {
                newPositionOnAxis.y = m_currentPos.y - distanceY;
            } else if (positiveY && negativeY) {
                newPositionOnAxis.y = m_currentPos.y;
            } else if (!positiveY && !negativeY) {
                newPositionOnAxis.y -= distanceY;
            }

            SetPlayerPositionAndSize(new Vector2Int(newPositionOnAxis.x, newPositionOnAxis.y), newPlayerSize);
        }

        private Vector2Int GetNewPositionOnAxis(Vector2Int currentFinalPos, Vector2 nextDirection, Vector2Int nextPlayerSize) {
            var newX = currentFinalPos.x;
            var newY = currentFinalPos.y;
            
            //por enquanto só right e left implementado
            if (nextDirection == Vector2.right) {
               newX = currentFinalPos.x - nextPlayerSize.x;
            } else if (nextDirection == Vector2.left) {
                newX = currentFinalPos.x + nextPlayerSize.x;
            }
            
            return new Vector2Int(newX, newY);
        }

        //Maybe whe can use the same function to fit position, only change parameters
        private bool CanFitInPositionY(Vector2Int newPositionOnAxis, Vector2Int newPlayerSize,
            Vector2Int nextDirection, out int totalAvailableBlocks) {
            var availableBlocks = 0;
            for (var i = 0; i < newPlayerSize.x; i++) {
                m_levelManager.GetTotalAvailableBlockWithinDepth(newPositionOnAxis, nextDirection, newPlayerSize.y, 1, out availableBlocks);
                newPositionOnAxis.x += 1;
            }

            if (availableBlocks < newPlayerSize.y) {
                totalAvailableBlocks = 0;
                return false;
            }
            
            totalAvailableBlocks = availableBlocks;
            return true;
        }

//        private bool CanFitInOppositePositionY(Vector2Int newPositionOnAxis, Vector2Int newPlayerSize,
//            Vector2Int nextDirection, out int totalAvailableBlocks) {
//            var availableBlocks = 0;
//            for (var i = 0; i <= newPlayerSize.x; i++) {
//                m_levelManager.GetTotalAvailableBlockWithinDepth(newPositionOnAxis, nextDirection, newPlayerSize.y, 0,out availableBlocks);
//                newPositionOnAxis.x += 1;
//            }
//
//            if (availableBlocks <= 1) {
//                totalAvailableBlocks = 0;
//                return false;
//            }
//            totalAvailableBlocks = availableBlocks;
//            return true;
//        }

        private void SetPlayerPositionAndSize(Vector2Int pos, Vector2Int size) {
            m_finalPos = pos;
            m_currentSize = size;
        }
    }
}