using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DungeonSlime.Character;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Enviroment {
    public class RockStates : CharacterStates {
        
        [SerializeField] private Vector2Int m_initialPosition;

        private readonly Dictionary <CharacterForms, Vector2Int> m_slotsOnGrid = new Dictionary<CharacterForms, Vector2Int> {
            {CharacterForms.NORMAL, new Vector2Int(3, 3)},
        };

        private readonly Dictionary<Vector2, Vector2Int> m_collisionOffsetPosition = new Dictionary<Vector2, Vector2Int> {
            {Vector2.down, new Vector2Int(0, 2)},
            {Vector2.up, new Vector2Int(0, 0)},
            {Vector2.left, new Vector2Int(2, 0)},
            {Vector2.right, new Vector2Int(0, 0)},
        };
  
        protected override void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMoveRockCharacterWithId>(OnMoveRockCharacterWithId);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCollisionWithSpikes>(OnCollisionWithSpikes);
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
        
        private void OnCollisionWithSpikes(OnCollisionWithSpikes ev) {
            if (ev.ObjectId != Id) return;
            
            characterMovement.MovementSequence.Kill();
            animator.SetTrigger("dead");
            Invoke(nameof(OnFinishDeadAnimation), 1.3f);
        }

        
        public override Vector2Int GetNextSize(Vector2 nextDirection) {
            return new Vector2Int(3,3);
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

        public void MoveToDestination(Vector2Int slimeFinalPosition, Vector2Int slimeDirection, Vector2Int slimeFinalSize, bool rockShouldDie) {
            if (rockShouldDie) {
                characterMovement.MovementSequence.Kill();
                Invoke(nameof(OnFinishDeadAnimation), 1.3f);
                return;
            }
            
            //var directionToMove = GetAxisToMove(slimeFinalPosition, slimeDirection);
            var finalPosition = characterMovement.CurrentPosition;

            if (slimeDirection == Vector2Int.up) {
                //posição final da pedra = player final pos - tamanho do player + tamanho da pedra <calculo para subir a pedra>
                finalPosition.y = slimeFinalPosition.y + slimeFinalSize.y + m_slotsOnGrid[CharacterForms.NORMAL].y;
            } else if (slimeDirection == Vector2Int.down) {
                //posição final da pedra = player final pos + tamanho da pedra <calculo para pedra descer>
                finalPosition.y = slimeFinalPosition.y - m_slotsOnGrid[CharacterForms.NORMAL].y - 1;
            } else if (slimeDirection == Vector2Int.right) {
                finalPosition.x = slimeFinalPosition.x + slimeFinalSize.x + m_slotsOnGrid[CharacterForms.NORMAL].x;
            } else if (slimeDirection == Vector2Int.left) {
                finalPosition.x = slimeFinalPosition.x - m_slotsOnGrid[CharacterForms.NORMAL].x - 1;
            }
            
            characterMovement.CurrentFinalPosition = finalPosition;
            characterMovement.OnMove(slimeDirection, true, charType);
        }
        
        public Vector2Int GetAxisToMove(Vector2Int slimePosition, Vector2Int slimeDirection, Vector2Int slimeSize) {
            if (slimeDirection == Vector2Int.down || slimeDirection == Vector2Int.up) {
                slimePosition.x += slimeSize.x/2;
                return slimePosition.x > characterMovement.CurrentPosition.x ? Vector2Int.left : Vector2Int.right;
            }

            slimePosition.y += slimeSize.y/2;
            return slimePosition.y > characterMovement.CurrentPosition.y ? Vector2Int.down : Vector2Int.up;
        }
        
        private void OnCollisionEnter2D(Collision2D other) {
            if (((1 << other.gameObject.layer) & objectLayer) == 0) return;

            if (!characterMovement.IsMoving) return;
            
            characterMovement.StopMovement();
            var obj = other.collider.gameObject.GetComponent<RockStates>();
            var newPosition = obj.GetPivotPosition(characterMovement.CurrentDirection);
            characterMovement.CurrentFinalPosition = new Vector2Int(newPosition.x, newPosition.y);
            characterMovement.OnMove(characterMovement.CurrentDirection, true, CharacterType.Slime);
        }

        public void OnFinishDeadAnimation() {
            Destroy(gameObject);
        }
    }
}
