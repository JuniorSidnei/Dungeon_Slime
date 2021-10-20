using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using UnityEngine;

namespace DungeonSlime.Utils {

    public class Collision2DFinishLevelHandler : MonoBehaviour {
        public LayerMask objectLayer;
        private bool m_isColliding;
        public Vector3 boxSize;
        
        private void FixedUpdate() {
            var colliderBuffer = new Collider2D[2];
            var sizeColliderBuffer = Physics2D.OverlapBoxNonAlloc(transform.position, boxSize, 0, colliderBuffer, objectLayer);

            if (sizeColliderBuffer == 0|| m_isColliding) return;

            m_isColliding = true;
            GameManager.Instance.LoadNextScene();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            
            Gizmos.DrawWireCube(transform.position, boxSize);
        }

//        private void OnTriggerEnter2D(Collider2D other) {
//            if (((1 << other.gameObject.layer) & objectLayer) == 0) return;
//            
//            GameManager.Instance.LoadNextScene();
//        }
    }
}
