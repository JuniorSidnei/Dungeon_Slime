using DungeonSlime.Utils;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DungeonSlime.Managers {

    public class LevelDifficultyManager : MonoBehaviour {
        
        public TextMeshProUGUI adviseTxt;
        public RectTransform slimeSelector;
        public RectTransform normalBtn;
        public RectTransform hardBtn;

        private enum LevelDifficulty {
            NORMAL = 0,
            HARD
        }
        
        public void OnNormalSelected() {
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, normalBtn.transform.position.y, 0);
            adviseTxt.gameObject.SetActive(false);
        }

        public void OnHardSelected() {
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, hardBtn.transform.position.y, 0);
            adviseTxt.gameObject.SetActive(true);
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