using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Character;
using UnityEngine;

namespace DungeonSlime.Utils {

    public class OnRestartGame {  }

    public class OnFinishMovement {
        public OnFinishMovement(Vector2 currentDirection, int characterId, CharacterStates.CharacterType characterType) {
            CurrentDirection = currentDirection;
            CharacterId = characterId;
            CharacterType = characterType;
        }
        
        public Vector2 CurrentDirection;
        public readonly int CharacterId;
        public readonly CharacterStates.CharacterType CharacterType;
    }

    public class OnMoveCharacter {
        public OnMoveCharacter(Vector2Int direction) {
            Direction = direction;
        }

        public Vector2Int Direction;
    }

    public class OnCharacterCollision {
        public OnCharacterCollision(int characterId, Vector2Int collisionPosition, bool rockIsAbleToMove) {
            CharacterId = characterId;
            CollisionPosition = collisionPosition;
            RockIsAbleToMove = rockIsAbleToMove;
        }

        public readonly int CharacterId;
        public Vector2Int CollisionPosition;
        public readonly bool RockIsAbleToMove;
    }

    public class OnMoveRockCharacterWithId {
        public OnMoveRockCharacterWithId(int characterId, Vector2Int direction) {
            MovementDirection = direction;
            CharacterId = characterId;
        }

        public Vector2Int MovementDirection;
        public readonly int CharacterId;
    }

    public class OnRockUnableToMove {
        public OnRockUnableToMove(int rockId) {
            RockId = rockId;
        }

        public readonly int RockId;
    }

    public class OnCollisionWithSpikes {
        public OnCollisionWithSpikes(int objectId) {
            ObjectId = objectId;
        }

        public readonly int ObjectId;
    }
    
    public class OnLevelSelectionBack { }
    
    public class OnLevelSelectionDone { }
    
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