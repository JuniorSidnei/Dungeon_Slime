using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DungeonSlime.Scriptables;
using DungeonSlime.Utils;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DungeonSlime.Managers {
    public class GameManager : Singleton<GameManager> {

        public QueuedEventDispatcher GlobalDispatcher = new QueuedEventDispatcher();
        public DoTransitionController transitionController;
        public LevelManager LevelManager;

        [Header("pause settings")]
        public GameObject pausePanel;
        
        private AsyncOperation m_loadScene;

        private InputSource m_intputManager;
        
        public AudioClip buttonClickSFX;

        private void Awake() {
            m_intputManager = new InputSource();
            m_intputManager.Enable();

            m_intputManager.Slime.movement.performed += OnMove;
            m_intputManager.Slime.pause_game.performed += ctx => PauseGame();
            m_intputManager.Slime.restart_game.performed += ctx => LoadCurrentScene();
            m_intputManager.Slime.Submit.performed += ctx => ResumeGame();
            m_intputManager.Slime.Cancel.performed += ctx => BackToMenu();
        }

        private void OnEnable()  {
            StartCoroutine(WaitToFadeOut(0.5f));
        }
        
        private void Update() {
            GlobalDispatcher.DispatchAll();
        }

        private void OnMove(InputAction.CallbackContext ctx) {
            var ctxValue = ctx.ReadValue<Vector2>();
            var inputValue = Vector2Int.RoundToInt(ctxValue);
            
            if (inputValue == Vector2Int.zero) return;
            
            GlobalDispatcher.Emit(new OnMoveCharacter(inputValue)); 
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

        public void PauseGame() {
            AudioManager.Instance.PlaySFX(buttonClickSFX, 1);
            transitionController.DoTransitionIn(0.2f, () => {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            });
        }

        public void ResumeGame() {
            Time.timeScale = 1;
            transitionController.DoTransitionOut(0.2f, () => {
                pausePanel.SetActive(false);
            });
        }

        public void BackToMenu() {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
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
