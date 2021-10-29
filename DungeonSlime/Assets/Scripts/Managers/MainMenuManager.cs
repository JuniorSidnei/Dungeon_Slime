using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DungeonSlime.Managers {

    public class MainMenuManager : MonoBehaviour {

        private UserData m_userData;

        [Header("Menu Canvas")]
        public CanvasGroup mainMenuCanvas;
        
        public CanvasGroup levelSelectionCanvas;
        
        public CanvasGroup levelDifficultyCanvas;
        
        public CanvasGroup controlCanvas;
        
        public CanvasGroup optionsCanvas;
        
        public CanvasGroup creditsCanvas;
        
        
        public TextMeshProUGUI levelIndexText;
        public TextMeshProUGUI movementLimitText;
        public GameObject padlock;
        
        private int m_levelIndex = 1;
        private bool m_levelCanBePlayed;
        private AsyncOperation m_loadScene;
        
        private void Awake() {
            m_userData = SaveManager.LoadData();
        }
        
        private void OnUpdateLevelIndex(int value) {
            m_levelIndex += value;
            
            if (m_levelIndex <= 1) {
                m_levelIndex = 1;
            }

            if (m_levelIndex >= 22) {
                m_levelIndex = 22;
            }
            
            levelIndexText.text = m_levelIndex.ToString();
            padlock.SetActive(m_levelIndex > m_userData.normalLevelUnlocked);
            m_levelCanBePlayed = m_levelIndex <= m_userData.normalLevelUnlocked;
        }

        public void OnAdvanceLevelIndex(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            
            OnUpdateLevelIndex(1);
        }
        
        public void OnReturnLevelIndex(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            
            OnUpdateLevelIndex(-1);
        }
        
        public void OnSelectionBack(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            Debug.Log("apertei voltar");
        }

        public void OnSelectionDone(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            
            if (!m_levelCanBePlayed) return;
            
            StartCoroutine(LoadScene(string.Format("Level_0{0}", m_levelIndex)));
        }

        private IEnumerator LoadScene(string sceneToLoad) {
            m_loadScene = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
            
            while (!m_loadScene.isDone) {
                yield return null;
            }
        }
    }
}