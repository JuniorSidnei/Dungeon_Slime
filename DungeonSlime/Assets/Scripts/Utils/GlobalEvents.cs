using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    public class OnLoadNextScene {  }

    public class OnFinishMovement {
        public OnFinishMovement(Vector2 currentDirection) {
            CurrentDirection = currentDirection;
        }
        
        public Vector2 CurrentDirection;
    }

    public class OnMoveCharacter {
        public OnMoveCharacter(Vector2Int direction) {
            Direction = direction;
        }

        public Vector2Int Direction;
    }

    public class OnCharacterCollision { }
    
    public class OnUpdateSprite {
        public OnUpdateSprite(Sprite currentSprite, int spriteColliderIndex) {
            CurrentSprite = currentSprite;
            SpriteColliderIndex = spriteColliderIndex;
        }

        public Sprite CurrentSprite;
        public int SpriteColliderIndex;
    }
}