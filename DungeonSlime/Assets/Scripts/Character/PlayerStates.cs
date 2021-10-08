using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Character {

    public class PlayerStates : CharacterStates {
        [SerializeField] private List<Sprite> m_slimeSprites;
        
        private readonly Dictionary <CharacterForms, Vector2Int> m_slotsOnGrid = new Dictionary<CharacterForms, Vector2Int> {
            {CharacterForms.NORMAL, new Vector2Int(6, 6)},
            {CharacterForms.SEMI_STRETCHED_H, new Vector2Int(9, 3)},
            {CharacterForms.FULL_STRETCHED_H, new Vector2Int(12, 1)},
            {CharacterForms.SEMI_STRETCHED_V,  new Vector2Int(3, 9)},
            {CharacterForms.FULL_STRETCHED_V, new Vector2Int(1, 12)},
        };

        protected override void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMoveCharacter>(OnMove);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnFinishMovement>(OnFinishMovement);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterCollision>(OnCharacterCollision);
            CharacterForm = CharacterForms.NORMAL;
        }

        private void OnMove(OnMoveCharacter ev) {
            characterMovement.OnMove(ev.Direction);
        }
        
        private void OnFinishMovement(OnFinishMovement ev) {
            var (slimeForm, i) = GetIndexAndForm(ev.CurrentDirection);
            CharacterForm = slimeForm;
            animator.SetInteger("form", i);
            GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[i], i));
        }

        private void OnCharacterCollision(OnCharacterCollision ev) {
            characterMovement.StopMovement();
            //rock need to now his position on grid
            //the direction that player is moving
            //receive the rock game object that has been hitted
            //set the direction in the event and launch to him
            //need update currentPos to make on move work
            //characterMovement.OnMove(characterMovement.CurrentDirection);
        }
        
        public override Vector2Int GetNextSize(Vector2 nextDirection) {
            var (slimeForm, i) = GetIndexAndForm(nextDirection);
            return GetCurrentSize(slimeForm);
        }

        protected override Tuple<CharacterForms, int> GetIndexAndForm(Vector2 direction, CharacterForms forms = CharacterForms.NORMAL) {
            var index = 0;
   
            switch (CharacterForm) {
                case CharacterForms.NORMAL:
                    if (direction == Vector2.right || direction == Vector2.left) {
                        index = (int)CharacterForms.SEMI_STRETCHED_V;
                        forms = CharacterForms.SEMI_STRETCHED_V;
                    } else if (direction == Vector2.up || direction == Vector2.down) {
                       index = (int)CharacterForms.SEMI_STRETCHED_H;
                       forms = CharacterForms.SEMI_STRETCHED_H;
                    }
                    break;
                case CharacterForms.SEMI_STRETCHED_H:
                    if (direction == Vector2.right || direction == Vector2.left) {
                       index = (int)CharacterForms.NORMAL;
                       forms  = CharacterForms.NORMAL;
                    } else if (direction == Vector2.up || direction == Vector2.down) {
                       index = (int)CharacterForms.FULL_STRETCHED_H;
                       forms = CharacterForms.FULL_STRETCHED_H;
                    }
                    break;
                case CharacterForms.FULL_STRETCHED_H:
                    if (direction == Vector2.right || direction == Vector2.left) {
                        index = (int)CharacterForms.SEMI_STRETCHED_H;
                        forms = CharacterForms.SEMI_STRETCHED_H;
                    } else if (direction == Vector2.up || direction == Vector2.down) {
                        index = (int)CharacterForms.FULL_STRETCHED_H;
                        forms = CharacterForms.FULL_STRETCHED_H;
                    }
                    break;
                case CharacterForms.SEMI_STRETCHED_V:
                    if (direction == Vector2.right || direction == Vector2.left) {
                        index = (int)CharacterForms.FULL_STRETCHED_V;
                        forms = CharacterForms.FULL_STRETCHED_V;
                    } else if (direction == Vector2.up || direction == Vector2.down) {
                        index = (int)CharacterForms.NORMAL;
                        forms = CharacterForms.NORMAL;
                    }
                    break;
                case CharacterForms.FULL_STRETCHED_V:
                    if (direction == Vector2.right || direction == Vector2.left) {
                        index = (int)CharacterForms.FULL_STRETCHED_V;
                        forms = CharacterForms.FULL_STRETCHED_V;
                    } else if (direction == Vector2.up || direction == Vector2.down) {
                        index = (int)CharacterForms.SEMI_STRETCHED_V;
                        forms = CharacterForms.SEMI_STRETCHED_V;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var formAndIndex = new Tuple<CharacterForms, int>(forms, index);
            return formAndIndex;
        }

        protected override void OnCollisionEnter2D(Collision2D other) {

//            if (((1 << other.gameObject.layer) & objectLayer) == 0) return;
//            
//            Debug.Log("bati na pedra kk");
            //this mask must be the rock layer
            //stop movement if its a block and change shape and do all the stuff need
            //just simple call resolveCollision with the correct parameters
        }

        public override Vector2Int GetCurrentSize(CharacterForms currentForm) {  
            return m_slotsOnGrid[currentForm];
        }
        
        public override CharacterForms GetCurrentForm() {
            return CharacterForm;
        }
    }
}