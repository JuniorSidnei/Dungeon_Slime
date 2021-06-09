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
        
        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMove>(OnMove);
        }

        private void OnMove(OnMove ev) {
            if (m_moving) return;
            
            Vector2Int currentPlayerPosition = (Vector2Int) m_levelManager.wallMap.WorldToCell(transform.localPosition);
            if (m_levelManager.GetFarthestBlock(currentPlayerPosition, ev.Direction, out Vector2Int toPosition)) {
                m_moving = true;

                var cellSizeX = m_levelManager.wallMap.cellSize.x;
                var cellSizeY = m_levelManager.wallMap.cellSize.y;
 
                if (ev.Direction == Vector2Int.right) {
                    //para direita é: -0.5f
                    transform.DOMoveX(toPosition.x - cellSizeX / 2, m_speed).SetEase(Ease.InOutQuart).OnComplete(() => {
                        m_moving = false;
                        Debug.Log("movi pra: " + toPosition);
                    }); //para esquedaé: +1.5f
                } else if (ev.Direction == Vector2Int.left) {
                    transform.DOMoveX(toPosition.x + (cellSizeX + 0.5f), m_speed).SetEase(Ease.InOutQuart).OnComplete(() => {
                        m_moving = false;
                        Debug.Log("movi pra: " + toPosition);
                    });//para baixo é: + 1.5f
                } else if (ev.Direction == Vector2Int.down) {
                    transform.DOMoveY(toPosition.y + (cellSizeY + 0.5f), m_speed).SetEase(Ease.InOutQuart).OnComplete(() => {
                        m_moving = false;
                        Debug.Log("movi pra: " + toPosition);
                    });//para cima é: -0.5f
                } else if (ev.Direction == Vector2Int.up) {
                    transform.DOMoveY(toPosition.y - cellSizeY / 2, m_speed).SetEase(Ease.InOutQuart).OnComplete(() => {
                        m_moving = false;
                        Debug.Log("movi pra: " + toPosition);
                    });
                }
            }
        }
    }
}