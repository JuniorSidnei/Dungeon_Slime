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
    public class PlayerMovement : MonoBehaviour {
        
        [SerializeField] private float m_speed;
        [SerializeField] private Rigidbody2D m_rb;
        [SerializeField] private bool Moving;
        private bool m_colliding;

        private Vector2 m_currentDirection;
        
        private void OnCollisionEnter2D(Collision2D other) {
            Debug.Log("bati");
            
            if (Moving) {
                Debug.Log("lancei evento");
                GameManager.Instance.GlobalDispatcher.Emit(new OnCollisionDetected(other.gameObject.layer, m_currentDirection,
                    () => {
                        m_rb.AddForce(m_currentDirection * m_speed, ForceMode2D.Impulse);
                    }));
            }
            Moving = false;
        }

        private void OnCollisionStay2D(Collision2D other) {
            //todo:: check collision here and set moving false
            
            //Debug.Log("estou colado na parede");
        }

        private void OnTriggerEnter2D(Collider2D other) {
            GameManager.Instance.GlobalDispatcher.Emit(new OnLoadNextScene());
        }

        private void Update() {
            
            if (Input.GetKeyDown(KeyCode.D) && !Moving && m_currentDirection != Vector2.right) {
                m_currentDirection = Vector2.right;
                m_rb.AddForce(m_currentDirection * m_speed, ForceMode2D.Impulse);
                Moving = true;
            }

            if (Input.GetKeyDown(KeyCode.A) && !Moving && m_currentDirection != Vector2.left) {
                m_currentDirection = Vector2.left;
                m_rb.AddForce(m_currentDirection * m_speed, ForceMode2D.Impulse);
                Moving = true;
            }

            if (Input.GetKeyDown(KeyCode.W) && !Moving && m_currentDirection != Vector2.up) {
                m_currentDirection = Vector2.up;
                m_rb.AddForce(m_currentDirection * m_speed, ForceMode2D.Impulse);
                Moving = true;
            }

            if (Input.GetKeyDown(KeyCode.S) && !Moving && m_currentDirection != Vector2.down) {
                m_currentDirection = Vector2.down;
                m_rb.AddForce(m_currentDirection * m_speed, ForceMode2D.Impulse);
                Moving = true;
            }
        }
    }
}