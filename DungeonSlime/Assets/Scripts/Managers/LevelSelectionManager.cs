using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DungeonSlime.Utils;
using GameToBeNamed.Utils.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DungeonSlime.Managers {

    public class LevelSelectionManager : MonoBehaviour {
        
        public TextMeshProUGUI levelIndexText;
        public GameObject padlock;

        [Header("arrows")]
        public GameObject rightArrow;
        public GameObject leftArrow;

        [Header("sounds")]
        public AudioClip selectionLevel;
        public AudioClip selectionDone;
        public AudioClip selectionBack;
        
        
        private int m_levelIndex = 1;
        private bool m_levelCanBePlayed = true;
        private AsyncOperation m_loadScene;
        private static UserData m_userData;
        
        private void Awake() {
            m_userData = SaveManager.LoadData();
        }

        public void OnAdvanceLevelIndex(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            
            var ctxValue = ctx.ReadValue<Vector2>();
            var inputValue = Vector2Int.RoundToInt(ctxValue);

            if (m_userData.isSfxOn) {
                AudioController.Instance.Play(selectionLevel, AudioController.SoundType.SoundEffect2D);
            }
            OnUpdateLevelIndex(inputValue.x);
            OnAnimateArrow(inputValue.x);
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
            
            var userIndex = m_userData.levelDifficulty == LevelManager.LevelDifficulty.NORMAL ? m_userData.normalLevelUnlocked : m_userData.hardLevelUnlocked;
            
            padlock.SetActive(m_levelIndex > userIndex);
            m_levelCanBePlayed = m_levelIndex <= userIndex;
        }

        private void OnAnimateArrow(int value) {
            switch (value) {
                case 1:
                    rightArrow.transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f);
                    rightArrow.transform.DOScale(new Vector3(-1, 1, 1), 0.1f).SetEase(Ease.InQuad);
                    break;
                case -1:
                    leftArrow.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    leftArrow.transform.DOScale(new Vector3(1, 1, 1), 0.1f).SetEase(Ease.InQuad);
                    break;
            }
        }
        
        public void OnSelectionDone() {
            if (!m_levelCanBePlayed) return;
            if (m_userData.isSfxOn) {
                AudioController.Instance.Play(selectionDone, AudioController.SoundType.SoundEffect2D);
            }
            StartCoroutine(LoadScene($"Level_0{m_levelIndex}"));
        }

        public void BackMenu() {
            if (m_userData.isSfxOn) {
                AudioController.Instance.Play(selectionBack, AudioController.SoundType.SoundEffect2D);
            }
            SceneManager.LoadScene("LevelDifficultySelectionMenu");
        }
        
        private IEnumerator LoadScene(string sceneToLoad) {
            m_loadScene = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
            AudioController.Instance.Stop();
            while (!m_loadScene.isDone) {
                yield return null;
            }
        }
    }

}