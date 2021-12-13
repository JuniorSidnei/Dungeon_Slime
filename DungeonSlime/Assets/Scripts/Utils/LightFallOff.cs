using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace DungeonSlime.Utils {

    public class LightFallOff : MonoBehaviour {
        private Light2D m_light;
        [SerializeField] private float m_maxIntensity;

        private void Start() {
            m_light = GetComponent<Light2D>();
        }

        private void FixedUpdate() {
            m_light.intensity = Mathf.PingPong(Time.time, m_maxIntensity);
        }
    }
}