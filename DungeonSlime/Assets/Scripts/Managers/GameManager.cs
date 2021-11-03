using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DungeonSlime.Scriptables;
using DungeonSlime.Utils;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonSlime.Managers {
    public class GameManager : Singleton<GameManager> {

        public QueuedEventDispatcher GlobalDispatcher = new QueuedEventDispatcher();
        public DoTransitionController transitionController;
        public LevelManager LevelManager;
        private AsyncOperation m_loadScene;

        private void OnEnable()  {
            StartCoroutine(WaitToFadeOut(0.5f));
        }
        
        private void Update() {
            GlobalDispatcher.DispatchAll();
        }
        
        public void LoadCurrentScene() {
            transitionController.DoTransitionIn(0.5f, () => {
                StartCoroutine(LoadScene(string.Format("Level_0{0}", LevelManager.levelData.currentLevelData)));
            });
        }
        
        public void LoadNextScene() {
            SaveAllData();
            
            transitionController.DoTransitionIn(0.5f, () => {
                StartCoroutine(LoadScene(string.Format("Level_0{0}", LevelManager.levelData.nextLevelData)));
            });
        }

        private IEnumerator WaitToFadeOut(float time) {
            yield return new WaitForSeconds(time);
            transitionController.DoTransitionOut(1f);
        }

        private IEnumerator LoadScene(string sceneToLoad) {
            m_loadScene = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
            
            while (!m_loadScene.isDone) {
                yield return null;
            }
        }

        private void SaveAllData() {
            var currentData = SaveManager.LoadData();
            var currentLevel = LevelManager.levelData.currentLevelData;
            var normalUnlocked = currentData.normalLevelUnlocked;
            var hardUnlocked = currentData.hardLevelUnlocked;
            
            switch (currentData.levelDifficulty) {
                case 0:
                    normalUnlocked = currentLevel;
                    break;
                case 1:
                    hardUnlocked = currentLevel;
                    break;
            }

            var userData = new UserData {
                lastLevelPlayed = currentLevel, normalLevelUnlocked = normalUnlocked, hardLevelUnlocked = hardUnlocked, levelDifficulty = currentData.levelDifficulty,
                isFullScreen = currentData.isFullScreen, isMusicOn = currentData.isMusicOn, isSfxOn = currentData.isSfxOn
            };

            SaveManager.SaveData(userData);
        }
    }
}
