using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    public class OnUpdateSlimeForm {  }

    public class OnCollisionDetected {
        public OnCollisionDetected(int objectLayer, Vector2 currentDirection) {
            ObjectLayer = objectLayer;
            CurrentDirection = currentDirection;
        }

        public int ObjectLayer;
        public Vector2 CurrentDirection;
    }
}