using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Character {
    public class OnTrigger : MonoBehaviour {

        public LayerMask objectLayer;

        private void OnTriggerEnter2D(Collider2D other) {
           
            Debug.Log("bati aqui caralho");    
        }
    }
}