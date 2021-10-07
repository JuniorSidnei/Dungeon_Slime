using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using UnityEngine;

namespace DungeonSlime.Utils {

    public class Collision2DFinishLevelHandler : MonoBehaviour {
        public LayerMask objectLayer;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (((1 << other.gameObject.layer) & objectLayer) == 0) return;
            
            GameManager.Instance.LoadNextScene();
        }
    }
}
