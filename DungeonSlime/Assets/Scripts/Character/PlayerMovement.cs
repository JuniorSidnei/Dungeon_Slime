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

        [SerializeField] private float m_speed;
        [SerializeField] private Transform m_hit_right_up;
        [SerializeField] private Transform m_hit_right_down;
        [SerializeField] private Transform m_hit_left_up;
        [SerializeField] private Transform m_hit_left_down;
        [SerializeField] private LayerMask m_groundMask;
        [SerializeField] private BoxCollider2D m_collider;
        public float OffsetDistance;
        public Ease EaseMovement;

        private void Update() {
            if (Input.GetKeyDown(KeyCode.D)) {
                transform.DOMoveX(
                    evaluateRaycast(m_hit_right_up.transform.position, m_hit_right_down, Vector2.right, Vector2.down).transform.position
                        .x -
                    OffsetDistance,
                    m_speed).SetEase(EaseMovement).OnComplete(() => {
                    GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSlimeForm());
                });
            }

            if (Input.GetKeyDown(KeyCode.A)) {
                transform.DOMoveX(
                    evaluateRaycast(m_hit_left_up.transform.position, m_hit_left_down, Vector2.left, Vector2.down).transform.position.x +
                    OffsetDistance,
                    m_speed).SetEase(EaseMovement);
            }

            if (Input.GetKeyDown(KeyCode.W)) {
                transform.DOMoveY(
                    evaluateRaycast(m_hit_right_up.transform.position, m_hit_left_up, Vector2.up, Vector2.left).transform.position.y -
                    OffsetDistance,
                    m_speed).SetEase(EaseMovement);
            }

            if (Input.GetKeyDown(KeyCode.S)) {
                transform.DOMoveY(
                    evaluateRaycast(m_hit_right_down.transform.position, m_hit_left_down, Vector2.down, Vector2.left).transform.position
                        .y +
                    OffsetDistance,
                    m_speed).SetEase(EaseMovement);
            }

            for (int i = 0; i < 5; i++) {
                Debug.DrawRay((Vector2)m_hit_right_up.transform.position + 0.35f * i * Vector2.down, Vector2.right, Color.red);
            }
        }

        private RaycastHit2D evaluateRaycast(Vector2 originHit, Transform secondHit, Vector2 rayDirection,
            Vector2 raySense) {
            var rayLenght = 50f;
            var raySpacing = .35f;
            var rayCount = 5; //maybe change this value based on slime form?
            var oldDistance = 500f;
            var minorHitDistance = new RaycastHit2D();
            Vector2 rayOrigin = originHit;
            
            for (var i = 0; i < rayCount; i++) {
                var hit = Physics2D.Raycast(rayOrigin + raySpacing * i * raySense, rayDirection, rayLenght, m_groundMask);
            
                Debug.DrawRay(rayOrigin + raySpacing * i * raySense, rayDirection, Color.red);
               
                if (hit) {
                    //Debug.Log("bati no: " + hit.collider.name);
                    var minorDistance = (Vector3)originHit - hit.transform.position;

                    if (minorDistance.magnitude < oldDistance) {
                        oldDistance = minorDistance.magnitude;
                        minorHitDistance = hit;
                        //Debug.Log("menor: " + hit.collider.name);
                    }
                }
            }

            return minorHitDistance;
        }
    }
}