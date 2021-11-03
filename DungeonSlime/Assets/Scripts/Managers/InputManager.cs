using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonSlime.Managers {

    public class InputManager : MonoBehaviour {
        public void Move(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            
            
        }

        public void PauseGame(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;

            GameManager.Instance.PauseGame();
        }
    }
}