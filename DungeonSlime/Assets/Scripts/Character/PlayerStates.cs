using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using GameToBeNamed.Utils.Sound;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DungeonSlime.Character {

    public class PlayerStates : CharacterStates
    {
        private int m_rockObjectId;
        public SlimeColliderMesh slimeColliderMesh;
        private int m_numberOfMovements;

        private static Vector2Int m_normalSize = new Vector2Int(6, 6);
        private static Vector2Int m_semi_stretched_H = new Vector2Int(9, 3);
        private static Vector2Int m_full_stretched_H = new Vector2Int(11, 1);
        private static Vector2Int m_semi_stretched_V = new Vector2Int(3, 9);
        private static Vector2Int m_full_stretched_V = new Vector2Int(1, 11);

        public Vector2Int NormalSize => m_normalSize;
        public Vector2Int SemiStretchedSizeHorizontal => m_semi_stretched_H;
        public Vector2Int FullStretchedSizeHorizontal => m_full_stretched_H;
        public Vector2Int SemiStretchedSizeVertical => m_semi_stretched_V;
        public Vector2Int FullStretchedSizeVertical => m_full_stretched_V;
        
        
        private readonly Dictionary <CharacterForms, Vector2Int> m_slotsOnGrid = new Dictionary<CharacterForms, Vector2Int> {
            {CharacterForms.NORMAL, m_normalSize},
            {CharacterForms.SEMI_STRETCHED_H, m_semi_stretched_H},
            {CharacterForms.FULL_STRETCHED_H, m_full_stretched_H},
            {CharacterForms.SEMI_STRETCHED_V,  m_semi_stretched_V},
            {CharacterForms.FULL_STRETCHED_V, m_full_stretched_V},
        };

        protected override void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnMoveCharacter>(OnMove);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnFinishMovement>(OnFinishMovement);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnRockUnableToMove>(OnRockUnableToMove);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCharacterCollision>(OnCharacterCollision);
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCollisionWithSpikes>(OnCollisionWithSpikes);
            characterMovement.SetLevelManager(levelManager);
            
            m_numberOfMovements = levelManager.levelData.numberOfMovements;
        }

        protected override void Start() {
            CharacterForm = CharacterForms.NORMAL;
            charType = CharacterType.Slime;
            characterMovement.SetCharacterId(Id);
        }
        
        private void OnMove(OnMoveCharacter ev) {
            if (!slimeColliderMesh.CanIMoveWithinDirection(ev.Direction)) {
                slimeColliderMesh.IsPlayerMoving = false;
                return;
            }
            
            slimeColliderMesh.IsPlayerMoving = true;
            slimeColliderMesh.ResetRocksId();
            characterMovement.OnMove(ev.Direction, false, charType);
            animator.SetBool("moving", characterMovement.IsMoving);
            animator.SetInteger("moveX", characterMovement.CurrentDirection.x);
            animator.SetInteger("moveY", characterMovement.CurrentDirection.y);

            if (levelManager.UserData.levelDifficulty != 1) return;
            
            m_numberOfMovements -= 1;
            levelManager.UpdateMovementLimitValue(m_numberOfMovements);
        }

        private void OnFinishMovement(OnFinishMovement ev) {
            if (ev.CharacterId != Id) return;
            
            slimeColliderMesh.IsPlayerMoving = false;
            slimeColliderMesh.IsPlayerColliding = false;
            var (slimeForm, i) = GetIndexAndForm(ev.CurrentDirection);
            CharacterForm = slimeForm;
            animator.SetBool("moving", characterMovement.IsMoving);
            animator.SetInteger("form", i);
            
            if (m_numberOfMovements <= 0 && !levelManager.IsLevelClear) {
                var randDeath = Random.Range(0, deathSounds.Length);
                AudioController.Instance.Play(deathSounds[randDeath], AudioController.SoundType.SoundEffect2D);
                animator.SetTrigger("dead");
                var spriteCenter = slimeColliderMesh.spriteRenderer.bounds.center;
                Instantiate(deadAnimation, spriteCenter, Quaternion.identity, transform);
                GameManager.Instance.LoadCurrentScene();
            }
        }

        private void OnRockUnableToMove(OnRockUnableToMove ev) {
            m_rockObjectId = ev.RockId;
        }
        
        private void OnCharacterCollision(OnCharacterCollision ev) {
            if (m_rockObjectId == ev.CharacterId) return;
            
            characterMovement.StopMovement();
            characterMovement.CurrentFinalPosition = ev.CollisionPosition;
            characterMovement.OnMove(characterMovement.CurrentDirection, true, charType);
            
            if (!ev.RockIsAbleToMove) return;
            
            var currentDirection = characterMovement.CurrentDirection;
            GameManager.Instance.GlobalDispatcher.Emit(new OnMoveRockCharacterWithId(ev.CharacterId, currentDirection));
        }

        private void OnCollisionWithSpikes(OnCollisionWithSpikes ev) {
            if (ev.ObjectId != Id) return;

            var randDeath = Random.Range(0, deathSounds.Length);
            AudioController.Instance.Play(deathSounds[randDeath], AudioController.SoundType.SoundEffect2D);
            animator.SetTrigger("dead");
            var spriteCenter = slimeColliderMesh.spriteRenderer.bounds.center;
            Instantiate(deadAnimation, spriteCenter, Quaternion.identity, transform);
            GameManager.Instance.LoadCurrentScene();
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

        public override Vector2Int GetCurrentSize(CharacterForms currentForm) {  
            return m_slotsOnGrid[currentForm];
        }
        
        public override CharacterForms GetCurrentForm() {
            return CharacterForm;
        }
    }
}