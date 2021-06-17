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
        public Ease ease;
        
        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMove>(OnMove);
        }

        private void OnMove(OnMove ev) {
            if (m_moving) return;
            
            
            var currentPlayerPosition = (Vector2Int) m_levelManager.wallMap.WorldToCell(transform.localPosition);
            if (m_levelManager.GetFarthestBlock(currentPlayerPosition, ev.Direction, out Vector2Int toPosition, out Block farthestBlock)) {
                m_moving = true;
                
                m_sprite = m_playerStates.GetNextSpriteToMovement(ev.Direction);
               
                var cellSize = m_levelManager.wallMap.cellSize;
                var spriteHalfSize = m_sprite.bounds.size / 2;
           
                m_speed = Vector2.Distance(toPosition, currentPlayerPosition) / 15;

                if (ev.Direction == Vector2Int.right) {
                    //para direita é: -0.5f (cellSizeX / 2) = 1/2
                    transform.DOMoveX(toPosition.x - spriteHalfSize.x, m_speed).SetEase(ease).OnComplete(() => {
                        m_moving = false;
                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(farthestBlock, ev.Direction));
                    }); //para esquedaé: +1.5f ((cellSizeX + 0.5f) = 1 + 0.5
                } else if (ev.Direction == Vector2Int.left) {
                    transform.DOMoveX(toPosition.x + (spriteHalfSize.x + cellSize.x), m_speed).SetEase(ease).OnComplete(() => {
                        m_moving = false;
                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(farthestBlock, ev.Direction));
                    });//para baixo é: + 1.5f (cellSizeY + 0.5f) = 1 + 0.5
                } else if (ev.Direction == Vector2Int.down) {
                    transform.DOMoveY(toPosition.y + (spriteHalfSize.y + cellSize.x), m_speed).SetEase(ease).OnComplete(() => {
                        m_moving = false;
                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(farthestBlock, ev.Direction));
                    });//para cima é: -0.5f cellSizeY / 2 = 1/2
                } else if (ev.Direction == Vector2Int.up) {
                    transform.DOMoveY(toPosition.y - spriteHalfSize.y, m_speed).SetEase(ease).OnComplete(() => {
                        m_moving = false;
                        GameManager.Instance.GlobalDispatcher.Emit(new OnFinishMovement(farthestBlock, ev.Direction));
                    });
                }
            }
        }
    }
}