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
        private Vector3 m_initialPos;
        private Vector2Int m_finalPos;
        private Block m_farthestBlock;
        public Ease ease;

        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMove>(OnMove);
        }

        private void OnMove(OnMove ev) {
            if (m_moving) return;

            m_sprite = m_playerStates.GetNextSpriteToMovement(ev.Direction);
            
            CalculateInitialPos();
            GetPositionToMove(ev.Direction);
            
            var cellSize = m_levelManager.wallMap.cellSize; 
            var spriteHalfSize = m_sprite.bounds.size / 2;
            
            var newPos = m_levelManager.wallMap.CellToLocal(new Vector3Int(m_finalPos.x, m_finalPos.y, 0));
            
            if (ev.Direction == Vector2Int.right) {
                //para direita é: -0.5f (cellSizeX / 2) = 1/2
                transform.DOMoveX(newPos.x - spriteHalfSize.x, m_speed).SetEase(ease).OnComplete(() => {
                    m_moving = false;
                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
                }); //para esquedaé: +1.5f ((cellSizeX + 0.5f) = 1 + 0.5
            }
            else if (ev.Direction == Vector2Int.left) {
                transform.DOMoveX(newPos.x + (spriteHalfSize.x + cellSize.x), m_speed).SetEase(ease).OnComplete(() => {
                    m_moving = false;
                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
                }); //para baixo é: + 1.5f (cellSizeY + 0.5f) = 1 + 0.5
            }
            else if (ev.Direction == Vector2Int.down) {
                transform.DOMoveY(newPos.y + (spriteHalfSize.y + cellSize.x), m_speed).SetEase(ease).OnComplete(() => {
                    m_moving = false;
                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
                }); //para cima é: -0.5f cellSizeY / 2 = 1/2
            }
            else if (ev.Direction == Vector2Int.up) {
                transform.DOMoveY(newPos.y - spriteHalfSize.y, m_speed).SetEase(ease).OnComplete(() => {
                    m_moving = false;
                    GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(m_farthestBlock, ev.Direction));
                });
            }

//            if (m_levelManager.GetFarthestBlock(currentPlayerPosition, ev.Direction, out Vector2Int toPosition, out Block farthestBlock)) {
//                m_moving = true;
//                
//                var cellSize = m_levelManager.wallMap.cellSize;
//                var spriteHalfSize = m_sprite.bounds.size / 2;
//                
//                m_speed = Vector2.Distance(toPosition, currentPlayerPosition) / 45;
//
//                var newPos = m_levelManager.wallMap.CellToLocal(new Vector3Int(toPosition.x, toPosition.y, 0));
//                if (ev.Direction == Vector2Int.right) {
//                    //para direita é: -0.5f (cellSizeX / 2) = 1/2
//                    transform.DOMoveX(newPos.x - spriteHalfSize.x, m_speed).SetEase(ease).OnComplete(() => {
//                        m_moving = false;
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(farthestBlock, ev.Direction));
//                    }); //para esquedaé: +1.5f ((cellSizeX + 0.5f) = 1 + 0.5
//                } else if (ev.Direction == Vector2Int.left) {
//                    transform.DOMoveX(newPos.x + (spriteHalfSize.x + cellSize.x), m_speed).SetEase(ease).OnComplete(() => {
//                        m_moving = false;
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(farthestBlock, ev.Direction));
//                    });//para baixo é: + 1.5f (cellSizeY + 0.5f) = 1 + 0.5
//                } else if (ev.Direction == Vector2Int.down) {
//                    transform.DOMoveY(newPos.y + (spriteHalfSize.y + cellSize.x), m_speed).SetEase(ease).OnComplete(() => {
//                        m_moving = false;
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(farthestBlock, ev.Direction));
//                    });//para cima é: -0.5f cellSizeY / 2 = 1/2
//                } else if (ev.Direction == Vector2Int.up) {
//                    transform.DOMoveY(newPos.y - spriteHalfSize.y, m_speed).SetEase(ease).OnComplete(() => {
//                        m_moving = false;
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(farthestBlock, ev.Direction));
//                    });
//                }
//            }
        }

        
        private void CalculateInitialPos() {
            var valueX = (transform.position.x - (0.5f * m_sprite.bounds.size.x));// + 0.25f;
            var valueY = (transform.position.y + (0.25f * m_sprite.bounds.size.y));// - 0.15f;
            m_initialPos = new Vector3(valueX, valueY);
        }
        
        private void GetPositionToMove(Vector2Int direction) {
            var currentPlayerPosition = (Vector2Int) m_levelManager.wallMap.WorldToCell(m_initialPos);
            
            var distance = 5000.0;

            if (direction == Vector2.right || direction == Vector2Int.left) {
                var amount = m_playerStates.getSlotsOnGridY(m_playerStates.GetCurrentForm());

                for (var i = 0; i < amount; i++) {
                    if (m_levelManager.GetFarthestBlock(currentPlayerPosition, direction, out Vector2Int toPosition, out Block farthestBlock)) {
                        var newDistance = Vector2.Distance(toPosition, currentPlayerPosition);

                        if (newDistance < distance) {
                            distance = newDistance;
                            m_finalPos = toPosition;
                            m_farthestBlock = farthestBlock;
                            m_speed = (float) (distance / 45);
                        }

                        currentPlayerPosition.y -= 1;
                    }
                }
            }
            else if (direction == Vector2.up || direction == Vector2.down) {
                var amount = m_playerStates.getSlotsOnGridX(m_playerStates.GetCurrentForm());

                for (var i = 0; i < amount; i++) {
                    if (m_levelManager.GetFarthestBlock(currentPlayerPosition, direction, out Vector2Int toPosition, out Block farthestBlock)) {
                        var newDistance = Vector2.Distance(toPosition, currentPlayerPosition);

                        if (newDistance < distance) {
                            distance = newDistance;
                            m_finalPos = toPosition;
                            m_farthestBlock = farthestBlock;
                            m_speed = (float) (distance / 45);
                        }

                        currentPlayerPosition.x += i;
                    }
                }
            }

            //return m_finalPos;
        }
    }
}