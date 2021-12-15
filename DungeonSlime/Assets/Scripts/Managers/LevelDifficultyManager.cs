using System;
using DungeonSlime.Utils;
using GameToBeNamed.Utils.Sound;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DungeonSlime.Managers {

    public class LevelDifficultyManager : MonoBehaviour {
        
        public TextMeshProUGUI adviseTxt;
        
        [Header("slime selector")]
        public GameObject slimeSelector;
        public Animator slimeSelectorAnim;
        
        [Header("buttons")]
        public GameObject normalBtn;
        public GameObject hardBtn;

        [Header("audios")]
        public AudioClip selection;
        public AudioClip selectionDone;
        public AudioClip selectionBack;

        private static UserData m_userData;

        private void Awake() {
            m_userData = SaveManager.LoadData();
        }

        private enum LevelDifficulty {
            NORMAL = 0,
            HARD
        }
        
        public void OnNormalSelected() {
            AnimateSlimeSelector();
            normalBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, normalBtn.transform.position.y, 0);
            adviseTxt.gameObject.SetActive(false);
        }

        public void OnNormalDeselected() {
            normalBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        
        public void OnHardSelected() {
            AnimateSlimeSelector();
            hardBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, hardBtn.transform.position.y, 0);
            adviseTxt.gameObject.SetActive(true);
        }

        public void OnHardDeselected() {
            hardBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        
        public void OnNormalPressed() {
            SaveDifficultySelection(LevelManager.LevelDifficulty.NORMAL);
            SceneManager.LoadScene("LevelSelectionMenu");
        }
        
        public void OnHardPressed() {
            SaveDifficultySelection(LevelManager.LevelDifficulty.HARD);
            SceneManager.LoadScene("LevelSelectionMenu");
        }

        public void BackMenu() {
            if (m_userData.isSfxOn) {
                AudioController.Instance.Play(selectionBack, AudioController.SoundType.SoundEffect2D);    
            }
            SceneManager.LoadScene("MainMenu");
        }
        
        private void AnimateSlimeSelector() {
            if (m_userData.isSfxOn) {
                AudioController.Instance.Play(selection, AudioController.SoundType.SoundEffect2D);    
            }
            
            slimeSelectorAnim.SetTrigger("move");
        }
        
        private void SaveDifficultySelection(LevelManager.LevelDifficulty levelDifficulty) {
            if (m_userData.isSfxOn) {
                AudioController.Instance.Play(selectionDone, AudioController.SoundType.SoundEffect2D);
            }
            
            var userData = new UserData {
                lastLevelPlayed = m_userData.lastLevelPlayed, normalLevelUnlocked = m_userData.normalLevelUnlocked, hardLevelUnlocked = m_userData.hardLevelUnlocked,
                levelDifficulty = levelDifficulty, isFullScreen = m_userData.isFullScreen, isMusicOn = m_userData.isMusicOn, isSfxOn = m_userData.isSfxOn
            };
            
            SaveManager.SaveData(userData);
        }
    }
}