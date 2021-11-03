using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace DungeonSlime.Managers {

    public class OptionsManager : MonoBehaviour {

        public GameObject slimeSelector;
        [Header("text")]
        public TextMeshProUGUI fullScreenText;
        public TextMeshProUGUI musicText;
        public TextMeshProUGUI soundText;
        public TextMeshProUGUI creditsText;

        [Header("panels")]
        public GameObject optionsPanel;
        public GameObject creditsPanel;

        [Header("btn selections")]
        public GameObject fullScreenBtn;
        public GameObject creditsFakeBtn;
        
        private bool m_isFullScreen;
        private bool m_isSoundOn = true;
        private bool m_isMusicOn = true;

        public void OnFullScreenSelected() {
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, fullScreenText.transform.position.y);
        }

        public void OnMusicSelected() {
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, musicText.transform.position.y);
        }

        public void OnSoundSelected() {
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, soundText.transform.position.y);
        }

        public void OnCreditsSelected() {
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, creditsText.transform.position.y);    
        }
        
        public void OnFullScreenChange() {
            m_isFullScreen = !m_isFullScreen;
            fullScreenText.text = m_isFullScreen ? "ON" : "OFF";
            Screen.fullScreen = m_isFullScreen;
        }

        public void OnMusicChange() {
            m_isMusicOn = !m_isMusicOn;
            musicText.text = m_isMusicOn ? "ON" : "OFF";
        }

        public void OnSoundChange() {
            m_isSoundOn = !m_isSoundOn;
            soundText.text = m_isSoundOn ? "ON" : "OFF";
        }

        public void OnCreditsPressed() {
            optionsPanel.SetActive(false);
            creditsPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(creditsFakeBtn);
        }

        public void OnCreditsCancel() {
            optionsPanel.SetActive(true);
            creditsPanel.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(fullScreenBtn);
        }

        public void BackMenu() {
            var currentData = SaveManager.LoadData();
            var userData = new UserData {
                lastLevelPlayed = currentData.lastLevelPlayed, normalLevelUnlocked = currentData.normalLevelUnlocked, hardLevelUnlocked = currentData.hardLevelUnlocked,
                levelDifficulty = currentData.levelDifficulty, isFullScreen = m_isFullScreen, isMusicOn = m_isMusicOn, isSfxOn = m_isSoundOn
            };
            
            SaveManager.SaveData(userData);
            SceneManager.LoadScene("MainMenu");
        }
    }
}