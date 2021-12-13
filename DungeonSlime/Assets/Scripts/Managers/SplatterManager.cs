using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Character;
using GameToBeNamed.Utils;
using UnityEngine;

namespace DungeonSlime.Managers {

    public class SplatterManager : Singleton<SplatterManager> {

        public GameObject splatter_full_v;
        public GameObject splatter_full_h;
        public GameObject splatter_semi_v;
        public GameObject splatter_semi_h;
        public GameObject splatter_normal;
        
        private int m_sortingIndex = 3;

        public void CreateSplatter(Bounds spriteBounds, Vector2 currentDirection,
            CharacterStates.CharacterForms currentForm, bool isBlockCollision) {
            
            if (isBlockCollision) return;
            
            var (splatterPos, splatterRotation) = CalculateSplatterPositionAndRotation(currentDirection, spriteBounds);
            var splatter = GetSplatter(currentForm, currentDirection);

            var splatterRenderer = Instantiate(splatter, splatterPos, Quaternion.Euler(new Vector3(0, 0, splatterRotation)), transform).GetComponent<SpriteRenderer>();
            
            splatterRenderer.sortingOrder = m_sortingIndex;
            m_sortingIndex++;
        }

        private Tuple<Vector2, float> CalculateSplatterPositionAndRotation(Vector2 currentDirection, Bounds spriteBounds) {
            var posX = 0.0f;
            var posY = 0.0f;
            var rotation = 0;
            
            if (currentDirection == Vector2.left) {
                posX = spriteBounds.min.x;
                posY = spriteBounds.center.y;
                rotation = -180;
            } else if (currentDirection == Vector2.right) {
                posX = spriteBounds.center.x;
                posY = spriteBounds.center.y;
            } else if (currentDirection == Vector2.down) {
                posX = spriteBounds.center.x + 0.3f;
                posY = spriteBounds.min.y;
                rotation = -90;
            } else if (currentDirection == Vector2.up) {
                posX = spriteBounds.center.x + 0.3f;
                posY = spriteBounds.max.y - 0.5f;
                rotation = 90;
            }

            var positionAndRotation = new Tuple<Vector2, float>(new Vector2(posX, posY), rotation);
            return positionAndRotation;    
        }

        private GameObject GetSplatter(CharacterStates.CharacterForms currentForm, Vector2 currentDirection) {
            //TODO?? NEED TO FIX SPLATTERS BY FORM AND DIRECTION
            switch (currentForm) {
                case CharacterStates.CharacterForms.SEMI_STRETCHED_H:
                    if (currentDirection == Vector2.left || currentDirection == Vector2.right) {
                        return splatter_normal;
                    }
                    else {
                        return splatter_full_v;
                    }
                case CharacterStates.CharacterForms.FULL_STRETCHED_H:
                    if (currentDirection == Vector2.left || currentDirection == Vector2.right) {
                        return splatter_semi_h;
                    }
                    else {
                        return splatter_full_v;
                    }
                case CharacterStates.CharacterForms.SEMI_STRETCHED_V:
                    if (currentDirection == Vector2.left || currentDirection == Vector2.right) {
                        return splatter_full_v;
                    }
                    else {
                        return splatter_normal;
                    }
                case CharacterStates.CharacterForms.FULL_STRETCHED_V:
                    if (currentDirection == Vector2.left || currentDirection == Vector2.right) {
                        return splatter_full_v;
                    }
                    else {
                        return splatter_semi_v;
                    }
                case CharacterStates.CharacterForms.NORMAL:
                    return splatter_normal;
            }

            return null;
        }
    }
}
