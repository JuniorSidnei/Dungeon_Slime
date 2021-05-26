using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DungeonSlime.Utils;
using GameToBeNamed.Utils;
using UnityEditor;
using UnityEngine;

namespace DungeonSlime.Character {
    public class PlayerMovement : MonoBehaviour {
        
        private float m_rayLenght = 50f;
        private float m_raySpacing = .35f;
        private int m_verticalRayCount;
        private int m_horizontalRayCount;
        private float m_oldDistance = 500f;

        [SerializeField] private float m_speed;
        [SerializeField] private Transform m_hit_right_up;
        [SerializeField] private Transform m_hit_right_down;
        [SerializeField] private Transform m_hit_left_up;
        [SerializeField] private Transform m_hit_left_down;
        [SerializeField] private LayerMask m_groundMask;
        [SerializeField] private SpriteRenderer m_sprite;
        public float OffsetDistance;
        public Ease EaseMovement;
 
        private void Start() {
            m_verticalRayCount = Mathf.RoundToInt(m_sprite.bounds.size.y / .25f);
            m_horizontalRayCount = Mathf.RoundToInt(m_sprite.bounds.size.x / .25f);
        }

        private void Update() {
            
            if (Input.GetKeyDown(KeyCode.D)) {
//                transform.DOMoveX(
//                    evaluateRaycast(m_hit_right_up.transform.position, m_verticalRayCount, Vector2.right, Vector2.down).transform.position
//                        .x -
//                    OffsetDistance,
//                    m_speed).SetEase(EaseMovement).OnComplete(() => {
//                    GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSlimeForm());
//                });
            }

            if (Input.GetKeyDown(KeyCode.A)) {
//                transform.DOMoveX(
//                    evaluateRaycast(m_hit_left_up.transform.position, m_verticalRayCount, Vector2.left, Vector2.down).transform.position.x +
//                    OffsetDistance,
//                    m_speed).SetEase(EaseMovement);
            }

            if (Input.GetKeyDown(KeyCode.W)) {
//                transform.DOMoveY(
//                    evaluateRaycast(m_hit_right_up.transform.position, m_horizontalRayCount, Vector2.up, Vector2.left).transform.position.y -
//                    OffsetDistance,
//                    m_speed).SetEase(EaseMovement);
            }

            if (Input.GetKeyDown(KeyCode.S)) {
//                transform.DOMoveY(
//                    evaluateRaycast(m_hit_right_down.transform.position, m_horizontalRayCount, Vector2.down, Vector2.left).transform.position
//                        .y +
//                    OffsetDistance,
//                    m_speed).SetEase(EaseMovement);
            }

//            var value = Mathf.RoundToInt(m_sprite.bounds.size.y / .35f);
//            for (var i = 0; i < value; i++) {
//                Debug.DrawRay((Vector2)m_hit_left_up.transform.position + m_raySpacing* i * Vector2.down, Vector2.left, Color.red);
//            }
//            
//            var value2 = Mathf.RoundToInt(m_sprite.bounds.size.x / .35f);
//            for (var i = 0; i < value2; i++) {
//                Debug.DrawRay((Vector2)m_hit_right_up.transform.position + m_raySpacing* i * Vector2.left, Vector2.up, Color.blue);
//            }
        }

        private RaycastHit2D evaluateRaycast(Vector2 originHit, int rayCount, Vector2 rayDirection,
            Vector2 raySense) {

            m_oldDistance = 500f;
            var minorHitDistance = new RaycastHit2D();
            
            for (var i = 0; i < rayCount; i++) {
                var hit = Physics2D.Raycast(originHit + m_raySpacing * i * raySense, rayDirection, m_rayLenght, m_groundMask);

                Debug.DrawRay(originHit + m_raySpacing * i * raySense, rayDirection, Color.red);
               
                if (hit) {
                    Debug.Log("bati no: " + hit.collider.name);
                    var minorDistance = (Vector3)originHit - hit.transform.position;

                    if (minorDistance.magnitude < m_oldDistance) {
                        m_oldDistance = minorDistance.magnitude;
                        minorHitDistance = hit;
                        Debug.Log("menor: " + hit.collider.name);
                    }
                }
            }
            
            return minorHitDistance;
        }
    }
}