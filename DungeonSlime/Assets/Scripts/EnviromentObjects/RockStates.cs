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
        
        private Dictionary <CharacterForms, Vector2Int> m_slotsOnGrid = new Dictionary<CharacterForms, Vector2Int> {
            {CharacterForms.NORMAL, new Vector2Int(2, 2)},
        };

        private Dictionary<Vector2, Vector2Int> m_collisionPosition = new Dictionary<Vector2, Vector2Int> {
            {Vector2.down, new Vector2Int(0, 0)},
            {Vector2.up, new Vector2Int(0, 3)},
            {Vector2.left, new Vector2Int(0, 0)},
            {Vector2.right, new Vector2Int(3, 0)},
        };
            
        protected override void Awake() {
            CharacterForm = CharacterForms.NORMAL;
            characterMovement.SetInitialPosition(m_initialPosition);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMoveRockCharacterWithId>(OnMoveRockCharacterWithId);
            characterMovement.SetCharacterId(Id);
        }

        private void OnMoveRockCharacterWithId(OnMoveRockCharacterWithId ev) {
            if (ev.CharacterId != Id) return;
            
            characterMovement.OnMove(ev.MovementDirection, false);
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

        public Vector2Int GetPivotPosition(Vector2 direction) {
            //need to take the player size to calculate position
            var position = characterMovement.CurrentPosition;
            return position + m_collisionPosition[direction];
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
