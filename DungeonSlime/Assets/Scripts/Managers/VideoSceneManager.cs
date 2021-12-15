using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace DungeonSlime.Managers {

    public class VideoSceneManager : MonoBehaviour {
        public Button firstSelected;
        public VideoClip videoIntro;
        
        private void Awake() {
            firstSelected.Select();
            var time = videoIntro.length - 0.5f;
            Invoke(nameof(IntroFinished), (float)time);
        }

        private void IntroFinished() {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);    
        }
        
        public void OnSubmitPressed() {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}