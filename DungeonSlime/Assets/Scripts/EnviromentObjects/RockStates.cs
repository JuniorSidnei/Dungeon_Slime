using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Character;
using UnityEngine;

namespace DungeonSlime.Enviroment {
    public class RockStates : CharacterStates {
        
        private Dictionary <CharacterForms, Vector2Int> m_slotsOnGrid = new Dictionary<CharacterForms, Vector2Int> {
            {CharacterForms.NORMAL, new Vector2Int(2, 2)},
        };

        protected override void Awake() {
            CharacterForm = CharacterForms.NORMAL;
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

        protected override void OnCollisionEnter2D(Collision2D other) {
            
            if (((1 << other.gameObject.layer) & objectLayer) == 0) return;
            
            Debug.Log("player me bateu kk");
            //throw event to move the rock
            //with the same direction that player was moving
            //the mask must be the player layer
        }
    }
}
