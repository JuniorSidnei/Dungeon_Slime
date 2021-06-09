using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonSlime.Managers {

    public class InputManager : MonoBehaviour {
        
        public void Move(InputAction.CallbackContext ctx) {
            var ctxValue = ctx.ReadValue<Vector2>();
            
            Vector2Int inputValue = Vector2Int.RoundToInt(ctxValue);
            GameManager.Instance.GlobalDispatcher.Emit(new OnMove(inputValue));
        }
    }
}