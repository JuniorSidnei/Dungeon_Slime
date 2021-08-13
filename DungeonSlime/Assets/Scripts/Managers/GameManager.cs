using System;
using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonSlime.Managers {
    public class GameManager : Singleton<GameManager> {

        public QueuedEventDispatcher GlobalDispatcher = new QueuedEventDispatcher();
        public LevelManager m_levelManager;

        
        private void Update() {
            GlobalDispatcher.DispatchAll();
        }

        public void LoadNextScene(int nextLevelIndex) {
            //TODO:: make async operation with transition and loading scene
            SceneManager.LoadScene(string.Format("Level_0{0}", nextLevelIndex));
        }
    }
}
