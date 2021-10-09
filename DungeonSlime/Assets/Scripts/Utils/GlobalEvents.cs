using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    public class OnLoadNextScene {  }

    public class OnFinishMovement {
        public OnFinishMovement(Vector2 currentDirection, int characterId) {
            CurrentDirection = currentDirection;
            CharacterId = characterId;
        }
        
        public Vector2 CurrentDirection;
        public int CharacterId;
    }

    public class OnMoveCharacter {
        public OnMoveCharacter(Vector2Int direction) {
            Direction = direction;
        }

        public Vector2Int Direction;
    }

    public class OnCharacterCollision {
        public OnCharacterCollision(int characterId, Vector2Int collisionPosition) {
            CharacterId = characterId;
            CollisionPosition = collisionPosition;
        }

        public int CharacterId;
        public Vector2Int CollisionPosition;
    }

    public class OnMoveRockCharacterWithId {
        public OnMoveRockCharacterWithId(int characterId, Vector2Int direction) {
            MovementDirection = direction;
            CharacterId = characterId;
        }

        public Vector2Int MovementDirection;
        public int CharacterId;
    }
    
//    public class OnUpdateSprite {
//        public OnUpdateSprite(Sprite currentSprite, int spriteColliderIndex) {
//            CurrentSprite = currentSprite;
//            SpriteColliderIndex = spriteColliderIndex;
//        }
//
//        public Sprite CurrentSprite;
//        public int SpriteColliderIndex;
//    }
}