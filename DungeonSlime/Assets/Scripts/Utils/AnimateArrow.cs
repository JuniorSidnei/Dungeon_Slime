using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DungeonSlime.Utils
{

    public class AnimateArrow : MonoBehaviour
    {
        public float maxY;
        public float minY;
        public float speed;
        private bool isfinished = true;
        
        private void Start() {
            OnAnimate();
        }
        
        private void OnAnimate() {
            if (!isfinished) return;

            isfinished = false;
            transform.DOMoveY(transform.position.y + maxY, speed).OnComplete(() => {
                transform.DOMoveY(transform.position.y - minY, speed).OnComplete(() => {
                    isfinished = true;
                    OnAnimate();
                });
            });
        }
    }
}