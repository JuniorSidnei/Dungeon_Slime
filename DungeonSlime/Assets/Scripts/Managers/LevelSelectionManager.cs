using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DungeonSlime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DungeonSlime.Managers {

    public class LevelSelectionManager : MonoBehaviour {
        
        public TextMeshProUGUI levelIndexText;
        public GameObject padlock;

        private int m_levelIndex = 1;
        private bool m_levelCanBePlayed;
        private AsyncOperation m_loadScene;
        private UserData m_userData;

        private void Awake() {
            m_userData = SaveManager.LoadData();
        }

        public void OnAdvanceLevelIndex(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            
            var ctxValue = ctx.ReadValue<Vector2>();
            var inputValue = Vector2Int.RoundToInt(ctxValue);
            OnUpdateLevelIndex(inputValue.x);
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
        
        public void OnSelectionDone() {
            if (!m_levelCanBePlayed) return;
            
            StartCoroutine(LoadScene(string.Format("Level_0{0}", m_levelIndex)));
        }

        public void BackMenu() {
            SceneManager.LoadScene("LevelDifficultySelection");
        }
        
        private IEnumerator LoadScene(string sceneToLoad) {
            m_loadScene = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
            
            while (!m_loadScene.isDone) {
                yield return null;
            }
        }
    }

}