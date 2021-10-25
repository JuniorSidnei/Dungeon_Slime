using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Enviroment {

    public class ColliderMesh : MonoBehaviour {
        private SpriteRenderer m_spriteRend;
        public LayerMask objectLayer;
        private bool m_enableCollision = true;

        protected bool EnableCollision {
            get => m_enableCollision;
            set => m_enableCollision = value;
        }

        protected SpriteRenderer SpriteRend => m_spriteRend;

        private void Awake() {
            m_spriteRend = GetComponentInChildren<SpriteRenderer>();
        }
        
        private void OnDrawGizmos() {
            Gizmos.color = Color.magenta;
            if (!m_spriteRend) return;
            
            var spriteBounds = SpriteRend.bounds;
            var origin = new Vector2(spriteBounds.center.x, spriteBounds.center.y);
            var size = new Vector2(spriteBounds.size.x, spriteBounds.size.y);
            
            Gizmos.DrawWireCube(origin, size);
        }
    }
}