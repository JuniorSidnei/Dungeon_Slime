using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Scriptables;
using DungeonSlime.Utils;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonSlime.Managers {
    public class GameManager : Singleton<GameManager> {

        public QueuedEventDispatcher GlobalDispatcher = new QueuedEventDispatcher();
        public DoTransitionController transitionController;
        private AsyncOperation m_loadScene;

        private void OnEnable()  {
            StartCoroutine(WaitToFadeOut(2));
        }
        
        private void Update() {
            GlobalDispatcher.DispatchAll();
        }
        
        public void LoadNextScene(int nextLevelIndex) {
            transitionController.DoTransitionIn(0.5f, () => {
                StartCoroutine(LoadScene(string.Format("Level_0{0}", nextLevelIndex)));    
            });
        }

        IEnumerator WaitToFadeOut(float time) {
            yield return new WaitForSeconds(time);
            transitionController.DoTransitionOut(2f);
        }
        
        IEnumerator LoadScene(string sceneToLoad) {
            m_loadScene = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
            
            while (!m_loadScene.isDone) {
                yield return null;
            }
        }
    }
}
