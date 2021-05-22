using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameToBeNamed.Utils;
using UnityEditor;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {
    
    [SerializeField] private float m_speed;
    [SerializeField] private Transform m_hit_right_up;
    [SerializeField] private Transform m_hit_right_down;
    [SerializeField] private Transform m_hit_left_up;
    [SerializeField] private Transform m_hit_left_down;
    [SerializeField] private LayerMask m_groundMask;
    public float OffsetDistance;
    public float Speed;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.D)) {
            transform.DOMoveX(evaluateRaycast(m_hit_right_up, m_hit_right_down, Vector2.right).transform.position.x - OffsetDistance, 
                Speed).SetEase(Ease.Linear);
        }
        
        if (Input.GetKeyDown(KeyCode.A)) {
            transform.DOMoveX(evaluateRaycast(m_hit_left_up, m_hit_left_down, Vector2.left).transform.position.x + OffsetDistance,
                Speed).SetEase(Ease.Linear);
        }
        
        if (Input.GetKeyDown(KeyCode.W)) {
            transform.DOMoveY(evaluateRaycast(m_hit_right_up, m_hit_left_up, Vector2.up).transform.position.y - OffsetDistance,
                Speed).SetEase(Ease.Linear);
        }
        
        if (Input.GetKeyDown(KeyCode.S)) {
            transform.DOMoveY(evaluateRaycast(m_hit_right_down, m_hit_left_down, Vector2.down).transform.position.y + OffsetDistance,
                Speed).SetEase(Ease.Linear);
        }
    }

    private RaycastHit2D evaluateRaycast(Transform firstHit, Transform secondHit, Vector2 direction) {
        RaycastHit2D hit = Physics2D.Raycast(firstHit.position, direction, 30, m_groundMask);
        RaycastHit2D hit_two = Physics2D.Raycast(secondHit.position, direction, 30f, m_groundMask);

        var distance_one = Vector3.zero;
        var distance_two = Vector3.zero;

        if (hit) {
            Debug.Log("hit bateu no: " + hit.collider.name);
            distance_one = firstHit.position - hit.transform.position;
        }

        if (hit_two) {
            Debug.Log("hit2 bateu no: " + hit_two.collider.name);
            distance_two = secondHit.position - hit_two.transform.position;
        }

        if (distance_one.magnitude < distance_two.magnitude || distance_one.magnitude > distance_two.magnitude) {
            return hit_two;
        }

        return hit;
    }
}
