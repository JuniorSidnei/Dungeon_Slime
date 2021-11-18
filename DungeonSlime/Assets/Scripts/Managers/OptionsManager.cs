using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Utils;
using GameToBeNamed.Utils.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace DungeonSlime.Managers {

    public class OptionsManager : MonoBehaviour {

        [Header("selector settings")]
        public GameObject slimeSelector;
        public Animator slimeSelectorAnim;
        
        [Header("text")]
        public TextMeshProUGUI fullScreenText;
        public TextMeshProUGUI musicText;
        public TextMeshProUGUI soundText;
        public TextMeshProUGUI creditsText;
        public TextMeshProUGUI advanceText;

        [Header("panels")]
        public GameObject optionsPanel;
        public GameObject creditsPanel;

        [Header("btn selections")]
        public GameObject fullScreenBtn;
        public GameObject soundBtn;
        public GameObject musicBtn;
        public GameObject creditsBtn;
        public GameObject creditsFakeBtn;

        [Header("audio clips")] 
        public AudioClip selection;
        public AudioClip selectionDone;
        public AudioClip selectionBack;
        
        
        private bool m_isFullScreen;
        private bool m_isSoundOn = true;
        private bool m_isMusicOn = true;

        public void OnFullScreenSelected() {
            AnimateSlimeSelector();
            fullScreenBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            fullScreenText.color = Color.yellow;
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, fullScreenText.transform.position.y);
        }

        public void OnFullScreenDeselected() {
            fullScreenBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            fullScreenText.color = Color.white;
        }
        
        public void OnMusicSelected() {
            AnimateSlimeSelector();
            musicBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            musicText.color = Color.yellow;
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, musicText.transform.position.y);
        }

        public void OnMusicDeselected() {
            musicBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            musicText.color = Color.white;
        }
        
        public void OnSoundSelected() {
            AnimateSlimeSelector();
            soundBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            soundText.color = Color.yellow;
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, soundText.transform.position.y);
        }

        public void OnSoundDeselected()  {
            soundBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            soundText.color = Color.white;
        }
        
        public void OnCreditsSelected() {
            AnimateSlimeSelector();
            creditsBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            creditsText.color = Color.yellow;
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, creditsText.transform.position.y);    
        }

        public void OnCreditsDeselected() {
            creditsBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            creditsText.color = Color.white;
        }
        
        public void OnFullScreenChange() {
            m_isFullScreen = !m_isFullScreen;
            fullScreenText.text = m_isFullScreen ? "ON" : "OFF";
            Screen.fullScreen = m_isFullScreen;
        }

        public void OnMusicChange() {
            m_isMusicOn = !m_isMusicOn;
            musicText.text = m_isMusicOn ? "ON" : "OFF";
            
            switch (m_isMusicOn) {
                case true:
                    AudioController.Instance.UnMuteMusic();
                    break;
                case false:
                    AudioController.Instance.MuteMusic();
                    break;
            }
        }

        public void OnSoundChange() {
            m_isSoundOn = !m_isSoundOn;
            soundText.text = m_isSoundOn ? "ON" : "OFF";
            
            switch (m_isSoundOn) {
                case true:
                    AudioController.Instance.UnMuteSounds();
                    break;
                case false:
                    AudioController.Instance.MuteSounds();
                    break;
            }
        }

        public void OnCreditsPressed() {
            optionsPanel.SetActive(false);
            creditsPanel.SetActive(true);
            slimeSelector.SetActive(false);
            advanceText.gameObject.SetActive(false);
            OnCreditsDeselected();
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(creditsFakeBtn);
        }

        public void OnCreditsCancel() {
            optionsPanel.SetActive(true);
            creditsPanel.SetActive(false);
            slimeSelector.SetActive(true);
            advanceText.gameObject.SetActive(true);
            OnFullScreenSelected();
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
        
        private void AnimateSlimeSelector() {
            AudioController.Instance.Play(selection, AudioController.SoundType.SoundEffect2D);
            slimeSelectorAnim.SetTrigger("move");
        }
    }
}