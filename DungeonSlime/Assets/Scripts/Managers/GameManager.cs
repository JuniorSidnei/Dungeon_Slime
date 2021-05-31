using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace DungeonSlime.Utils {
    public class GameManager : Singleton<GameManager> {

        public QueuedEventDispatcher GlobalDispatcher = new QueuedEventDispatcher();
        public LevelManager m_levelManager;

        private void Start() {
            m_levelManager.LoadLevel(0);
        }

        private void Update() {
            GlobalDispatcher.DispatchAll();
        }
    }
}
