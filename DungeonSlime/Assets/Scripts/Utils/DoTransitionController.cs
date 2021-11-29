using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DungeonSlime.Managers;
using GameToBeNamed.Utils.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonSlime.Utils {
    public class DoTransitionController : MonoBehaviour {

        public Image imageTransition;
        public float offsetIn;
        public float offsetOut;
        public float transitionTimeIn;
        public float transitionTimeOut;
        public Vector2 resetPosition;
        public AudioClip sound;

        private static UserData m_userData;
        
        private void Awake() {
            m_userData = SaveManager.LoadData();
        }

        public void DoTransitionIn(Action onFinishTransition = null)  {
            imageTransition.gameObject.SetActive(true);
            if (m_userData.isSfxOn) {
                AudioController.Instance.Play(sound, AudioController.SoundType.SoundEffect2D);
            }

            imageTransition.transform.DOMoveX(offsetIn, transitionTimeIn).OnComplete(() => onFinishTransition?.Invoke());
        }

        public void DoTransitionOut(Action onFinishTransition = null) {
            if (m_userData.isSfxOn) {
                AudioController.Instance.Play(sound, AudioController.SoundType.SoundEffect2D);
            }

            imageTransition.transform.DOMoveX(offsetOut, transitionTimeOut).OnComplete(() => {
                onFinishTransition?.Invoke();
                imageTransition.rectTransform.anchoredPosition = resetPosition;
                imageTransition.gameObject.SetActive(false);
            });
        }
    }
}