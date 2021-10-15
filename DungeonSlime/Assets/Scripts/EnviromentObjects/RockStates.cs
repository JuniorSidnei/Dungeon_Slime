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

        private Dictionary<Vector2, Vector2Int> m_collisionOffsetPosition = new Dictionary<Vector2, Vector2Int> {
            {Vector2.down, new Vector2Int(0, 1)},
            {Vector2.up, new Vector2Int(0, -1)},
            {Vector2.left, new Vector2Int(1, 0)},
            {Vector2.right, new Vector2Int(0, 0)},
        };
  
        protected override void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMoveRockCharacterWithId>(OnMoveRockCharacterWithId);
            characterMovement.SetLevelManager(levelManager);
        }
        
        protected override void Start() {
            CharacterForm = CharacterForms.NORMAL;
            charType = CharacterType.Rock;
            characterMovement.SetInitialPosition(m_initialPosition);
            characterMovement.SetCharacterId(Id);
        }
        
        private void OnMoveRockCharacterWithId(OnMoveRockCharacterWithId ev) {
            if (ev.CharacterId != Id) return;

            characterMovement.OnMove(ev.MovementDirection, false, charType);
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
            var position = characterMovement.CurrentPosition;
            return position + m_collisionOffsetPosition[direction];
        }

        public void MoveToDestination(Vector2Int slimeFinalPosition, Vector2Int slimeDirection, Vector2Int slimeFinalSize) {
            var directionToMove = GetAxisToMove(slimeFinalPosition, slimeDirection);
            var finalPosition = new Vector2Int(characterMovement.CurrentPosition.x, 0);

            if (directionToMove == Vector2Int.up) {
                //posição final da pedra = player final pos - tamanho do player + tamanho da pedra <calculo para subir a pedra>
                finalPosition.y = slimeFinalPosition.y + slimeFinalSize.y + m_slotsOnGrid[CharacterForms.NORMAL].y;
            } else if (directionToMove == Vector2Int.down) {
                //posição final da pedra = player final pos + tamanho da pedra <calculo para pedra descer>
                finalPosition.y = slimeFinalPosition.y - m_slotsOnGrid[CharacterForms.NORMAL].y;
            }
            
            //set final pos
            characterMovement.CurrentFinalPosition = finalPosition;
            //call on move with true
            characterMovement.OnMove(directionToMove, true, charType);
        }
        
        public Vector2Int GetAxisToMove(Vector2Int slimePosition, Vector2Int slimeDirection) {
            if (slimeDirection == Vector2Int.down || slimeDirection == Vector2Int.up) {
                return slimePosition.x > characterMovement.CurrentPosition.x ? Vector2Int.left : Vector2Int.right;
            }
            
            return slimePosition.y > characterMovement.CurrentPosition.y ? Vector2Int.down : Vector2Int.up;
        }
        
        private void OnCollisionEnter2D(Collision2D other) {
            if (((1 << other.gameObject.layer) & objectLayer) == 0) return;

            if (!characterMovement.IsMoving) return;
            
            characterMovement.StopMovement();
            var obj = other.collider.gameObject.GetComponent<RockStates>();
            var newPosition = obj.GetPivotPosition(characterMovement.CurrentDirection);
            characterMovement.CurrentFinalPosition = new Vector2Int(newPosition.x, newPosition.y);
            characterMovement.OnMove(characterMovement.CurrentDirection, true, charType);
        }
    }
}
