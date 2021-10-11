using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Character;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Enviroment {
    public class RockStates : CharacterStates {
        
        [SerializeField] private Vector2Int m_initialPosition;
        private CharacterType m_charType = CharacterType.Rock;
        
        private Dictionary <CharacterForms, Vector2Int> m_slotsOnGrid = new Dictionary<CharacterForms, Vector2Int> {
            {CharacterForms.NORMAL, new Vector2Int(2, 2)},
        };

        private Dictionary<Vector2, Vector2Int> m_collisionOffsetPosition = new Dictionary<Vector2, Vector2Int> {
            {Vector2.down, new Vector2Int(0, 1)},
            {Vector2.up, new Vector2Int(0, -1)},
            {Vector2.left, new Vector2Int(-1, 0)},
            {Vector2.right, new Vector2Int(0, 0)},
        };
//            
        protected override void Awake() {
            CharacterForm = CharacterForms.NORMAL;
            characterMovement.SetInitialPosition(m_initialPosition);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMoveRockCharacterWithId>(OnMoveRockCharacterWithId);
            characterMovement.SetCharacterId(Id);
        }

        private void OnMoveRockCharacterWithId(OnMoveRockCharacterWithId ev) {
            if (ev.CharacterId != Id) return;
            
            characterMovement.OnMove(ev.MovementDirection, false, m_charType);
        }
        
        public override Vector2Int GetNextSize(Vector2 nextDirection) {
            return new Vector2Int(2,2);
        }

        public override Vector2Int GetCurrentSize(CharacterForms currentForm) {
            return m_slotsOnGrid[0];
        }

        public override CharacterForms GetCurrentForm() {
            return CharacterForm;
        }

        protected override Tuple<CharacterForms, int> GetIndexAndForm(Vector2 direction, CharacterForms forms = CharacterForms.NORMAL) {
            var formAndIndex = new Tuple<CharacterForms, int>(forms, 0);
            return formAndIndex;
        }

        public Vector2Int GetPivotPosition(Vector2 direction, Vector2Int slimeSize) {
            //need to take the player size to calculate position

            //return characterMovement.CurrentPosition;
            var position = characterMovement.CurrentPosition;

//            if (direction == Vector2.right) {
//                position.x -= slimeSize.x;
//            }
//            else {
//                position += m_collisionOffsetPosition[direction];
//            }
            
//            switch (direction) {
//                case Vector2 v when v.Equals(Vector2.right):
//                    position.x -= slimeSize.x;                
//                    break;
//                case Vector2 v when v.Equals(Vector2.left):
//                    position.x -= 2;                
//                    break;
//                case Vector2 v when v.Equals(Vector2.up):
//                    position.y -= 1;                
//                    break;
//                case Vector2 v when v.Equals(Vector2.down):
//                    position.y += mc;                
//                    break;
//            }

            //return position;

            return position + m_collisionOffsetPosition[direction];
        }
        
//        protected override void OnCollisionEnter2D(Collision2D other) {
//            
//            if (((1 << other.gameObject.layer) & objectLayer) == 0) return;
//            
//            Debug.Log("player me bateu kk");
//            //throw event to move the rock
//            //with the same direction that player was moving
//            //the mask must be the player layer
//        }
    }
}
