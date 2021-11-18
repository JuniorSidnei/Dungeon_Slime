using DungeonSlime.Utils;
using GameToBeNamed.Utils;
using GameToBeNamed.Utils.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DungeonSlime.Managers {

    public class MainMenuManager : MonoBehaviour {

        [Header("buttons selections")]
        public Button firstSelected;
        public Button controlsSelected;

        public GameObject textAdvance;
        
        [Header("slime selector settings")]
        public GameObject slimeSelector;
        public Animator slimeSelectorAnim;
        
        [Header("buttons settings")]
        public GameObject playBtn;
        public GameObject controlBtn;
        public GameObject optionsBtn;
        public GameObject quitBtn;

        [Header("menu panels")]
        public GameObject menuPanel;
        public GameObject controlsPanel;
        
        [Header("sounds")]
        public AudioClip menuTheme;
        public AudioClip selection;
        public AudioClip choose;

        private void Start() {
            if (!firstSelected) return;
            
            firstSelected.Select();
            AudioController.Instance.Play(menuTheme, AudioController.SoundType.Music, 0.25f, true);
        }

        public void OnPlaySelected() {
            AnimateSlimeSelector();
            playBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, playBtn.transform.position.y, 0);
        }
        
        public void OnPlayDeselected() {
            playBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        
        public void OnControlSelected() {
            AnimateSlimeSelector();
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, controlBtn.transform.position.y, 0);
        }
        
        public void OnControlDeselected() {
            controlBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        
        public void OnOptionsSelected() {
            AnimateSlimeSelector();
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, optionsBtn.transform.position.y, 0);
        }
        public void OnOptionsDeselected() {
            optionsBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        
        public void OnQuitSelected() {
            AnimateSlimeSelector();
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, quitBtn.transform.position.y, 0);
        }
        
        public void OnQuitDeselected() {
            quitBtn.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        
        public void OnPlayPressed() {
            AudioController.Instance.Play(choose, AudioController.SoundType.SoundEffect2D);
            SceneManager.LoadScene("LevelDifficultySelectionMenu");
        }
        
        public void OnControlPressed() {
            controlsSelected.Select();
            EnableControlsPanel(true);
        }
        
        public void OnOptionsPressed() {
            AudioController.Instance.Play(choose, AudioController.SoundType.SoundEffect2D);
            SceneManager.LoadScene("OptionsMenu");
        }
        
        public void OnQuitPressed() {
            AudioController.Instance.Play(choose, AudioController.SoundType.SoundEffect2D);
            Application.Quit();
        }

        public void OnQuitControls() {
            firstSelected.Select();
            OnPlaySelected();
            EnableControlsPanel(false);
        }
        
        private void AnimateSlimeSelector() {
            AudioController.Instance.Play(selection, AudioController.SoundType.SoundEffect2D);
            slimeSelectorAnim.SetTrigger("move");
        }

        private void EnableControlsPanel(bool enable)  {
            AudioController.Instance.Play(choose, AudioController.SoundType.SoundEffect2D);
            controlsPanel.SetActive(enable);
            menuPanel.SetActive(!enable);
            slimeSelector.SetActive(!enable);
            textAdvance.SetActive(!enable);
        }
     }
}