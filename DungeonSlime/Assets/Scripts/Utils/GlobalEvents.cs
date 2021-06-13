using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    public class OnLoadNextScene {  }

    public class OnFinishMovement {
        public OnFinishMovement(Block block, Vector2 currentDirection) {
            Block = block;
            CurrentDirection = currentDirection;
        }

        public Block Block;
        public Vector2 CurrentDirection;
    }

    public class OnMove {
        public OnMove(Vector2Int direction) {
            Direction = direction;
        }

        public Vector2Int Direction;
    }

    public class OnUpdateSprite {
        public OnUpdateSprite(Sprite currentSprite) {
            CurrentSprite = currentSprite;
        }

        public Sprite CurrentSprite;
    }
}