using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
  
        public void DoTransitionIn(Action onFinishTransition = null)  {
            imageTransition.gameObject.SetActive(true);
            imageTransition.transform.DOMoveX(offsetIn, transitionTimeIn).OnComplete(() => onFinishTransition?.Invoke());
        }

        public void DoTransitionOut(Action onFinishTransition = null) {
            imageTransition.transform.DOMoveX(offsetOut, transitionTimeOut).OnComplete(() => {
                onFinishTransition?.Invoke();
                imageTransition.rectTransform.anchoredPosition = resetPosition;
                imageTransition.gameObject.SetActive(false);
            });
        }
    }
}