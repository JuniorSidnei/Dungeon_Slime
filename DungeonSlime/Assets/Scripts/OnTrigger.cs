using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using DungeonSlime.Scriptables;
using UnityEngine;

namespace DungeonSlime.Character {
    public class OnTrigger : MonoBehaviour {

        public LevelData levelData;
        private void OnTriggerEnter2D(Collider2D other) {
            GameManager.Instance.LoadNextScene(levelData.NextLevelData);  
        }
    }
}