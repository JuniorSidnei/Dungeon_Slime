using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Character {

    public class PlayerStates : MonoBehaviour {

        enum SlimeForms {
            NORMAL,
            SEMI_STRECHED_H,
            FULL_STRETCHED_H,
            SEMI_STRETCHED_V,
            FULL_STRETCHED_V
        }

        private SlimeForms m_slimeForms;
        
        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnUpdateSlimeForm>(OnUpdateSlimeForm);
            m_slimeForms = SlimeForms.NORMAL;
        }

        private void OnUpdateSlimeForm(OnUpdateSlimeForm ev) {
            Debug.Log("mudando forma");
            transform.localScale = new Vector3(5, 20);
        }
    }
}