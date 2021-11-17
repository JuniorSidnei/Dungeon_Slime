using DungeonSlime.Utils;
using GameToBeNamed.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DungeonSlime.Managers {

    public class MainMenuManager : MonoBehaviour {

        public Button firstSelected;

        [Header("slime selector settings")]
        public GameObject slimeSelector;
        public Animator slimeSelectorAnim;
        
        [Header("buttons settings")]
        public GameObject playBtn;
        public GameObject controlBtn;
        public GameObject optionsBtn;
        public GameObject quitBtn;
        
        private void Start() {
            if (!firstSelected) return;
            
            firstSelected.Select();
        }

        public void OnPlaySelected() {
            AnimateSlimeSelector();
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, playBtn.transform.position.y, 0);
        }

        public void OnControlSelected() {
            AnimateSlimeSelector();
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, controlBtn.transform.position.y, 0);
        }

        public void OnOptionsSelected() {
            AnimateSlimeSelector();
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, optionsBtn.transform.position.y, 0);
        }

        public void OnQuitSelected() {
            AnimateSlimeSelector();
            slimeSelector.transform.position = new Vector3(slimeSelector.transform.position.x, quitBtn.transform.position.y, 0);
        }
        
        public void OnPlayPressed() {
            SceneManager.LoadScene("LevelDifficultySelectionMenu");
        }

        public void OnControlPressed() {
            Debug.Log("apertei control");
        }

        public void OnOptionsPressed() {
            SceneManager.LoadScene("OptionsMenu");
        }

        public void OnQuitPressed() {
            Application.Quit();
        }

        private void AnimateSlimeSelector()  {
            slimeSelectorAnim.SetTrigger("move");
        }
     }
}