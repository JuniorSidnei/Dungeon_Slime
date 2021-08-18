using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonSlime.Utils {
    public class DoTransitionController : MonoBehaviour {

        public Image imageTransition;

        public void DoTransitionIn(float transitionTime, Action onFinishTransition = null) {
            imageTransition.DOFade(1, transitionTime).OnComplete(() => onFinishTransition?.Invoke());
        }

        public void DoTransitionOut(float transitionTime, Action onFinishTransition = null) {
            imageTransition.DOFade(0, transitionTime).OnComplete(() => onFinishTransition?.Invoke());
        }
    }
}