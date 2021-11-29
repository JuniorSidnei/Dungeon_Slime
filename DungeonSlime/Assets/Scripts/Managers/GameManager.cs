using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DungeonSlime.Scriptables;
using DungeonSlime.Utils;
using GameToBeNamed.Utils;
using GameToBeNamed.Utils.Sound;
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
        
        [Header("audio settings")]
        public AudioClip buttonClickSfx;
        public AudioClip levelClip;

        private void Awake() {
            m_intputManager = new InputSource();
            m_intputManager.Enable();

            m_intputManager.Slime.movement.performed += OnMove;
            m_intputManager.Slime.pause_game.performed += ctx => PauseGame();
            m_intputManager.Slime.restart_game.performed += ctx => LoadCurrentScene();
            m_intputManager.Slime.Submit.performed += ctx => ResumeGame();
            m_intputManager.Slime.Cancel.performed += ctx => BackToMenu();
        }

        private void OnEnable() {
            var userData = SaveManager.LoadData();
            if (userData.isMusicOn) {
                AudioController.Instance.Play(levelClip, AudioController.SoundType.Music, 0.8f);
            }
            
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
            transitionController.DoTransitionIn(() => {
                StartCoroutine(LoadScene(string.Format("Level_0{0}", LevelManager.levelData.currentLevelData)));
            });
        }
        
        public void LoadNextScene() {
            SaveAllData();
            
            transitionController.DoTransitionIn(() => {
                StartCoroutine(LoadScene(string.Format("Level_0{0}", LevelManager.levelData.nextLevelData)));
            });
        }

        public void PauseGame() {
            AudioManager.Instance.PlaySFX(buttonClickSfx, 1);
            transitionController.DoTransitionIn(() => {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            });
        }

        public void ResumeGame() {
            Time.timeScale = 1;
            AudioManager.Instance.PlaySFX(buttonClickSfx, 1);
            pausePanel.SetActive(false);
            transitionController.DoTransitionOut();
        }

        public void BackToMenu() {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
        
        private IEnumerator WaitToFadeOut(float time) {
            yield return new WaitForSeconds(time);
            transitionController.DoTransitionOut();
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
