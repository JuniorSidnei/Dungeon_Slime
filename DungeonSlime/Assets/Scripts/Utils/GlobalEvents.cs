using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    public class OnLoadNextScene {  }

    public class OnCollisionDetected {
        public OnCollisionDetected(int objectLayer, Vector2 currentDirection, Action onFinishAnimation) {
            ObjectLayer = objectLayer;
            CurrentDirection = currentDirection;
            OnFinishAnimation = onFinishAnimation;
        }

        public int ObjectLayer;
        public Vector2 CurrentDirection;
        public Action OnFinishAnimation;
    }

    public class OnMove {
        public OnMove(Vector2Int direction) {
            Direction = direction;
        }

        public Vector2Int Direction;
    }
}