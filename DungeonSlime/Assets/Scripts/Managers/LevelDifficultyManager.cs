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
           SaveDifficultySelection(LevelDifficulty.NORMAL);
            SceneManager.LoadScene("LevelSelectionMenu");
        }
        
        public void OnHardPressed() {
            SaveDifficultySelection(LevelDifficulty.HARD);
            SceneManager.LoadScene("LevelSelectionMenu");
        }

        public void BackMenu() {
            SceneManager.LoadScene("MainMenu");
        }
        
        private void AnimateSlimeSelector() {
            AudioController.Instance.Play(selection, AudioController.SoundType.SoundEffect2D);
            slimeSelectorAnim.SetTrigger("move");
        }
        
        private void SaveDifficultySelection(LevelDifficulty levelDifficulty) {
            var currentData = SaveManager.LoadData();
            var userData = new UserData {
                lastLevelPlayed = currentData.lastLevelPlayed, normalLevelUnlocked = currentData.normalLevelUnlocked, hardLevelUnlocked = currentData.hardLevelUnlocked,
                levelDifficulty = (int) levelDifficulty
            };
            
            SaveManager.SaveData(userData);
        }
    }
}