using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using TMPro;
using UnityEngine;

namespace DungeonSlime.Character {

    public abstract class CharacterStates : MonoBehaviour {
        public Animator animator;
        public LayerMask objectLayer;
        public CharacterMovement characterMovement;
        public LevelManager levelManager;

        public CharacterType charType;
        
        [Header("sounds")]
        public AudioClip moveSound;
        public AudioClip stopMoveSound;
        
        [SerializeField] private int m_id;
        
        
        public int Id {
            get => m_id;
            set => m_id = value;
        }
        
        public enum CharacterForms {
            NORMAL,
            SEMI_STRETCHED_H,
            FULL_STRETCHED_H,
            SEMI_STRETCHED_V,
            FULL_STRETCHED_V
        }

        public enum CharacterType {
            Slime,
            Rock
        }
        
        protected abstract void Awake();

        protected abstract void Start();

        protected CharacterForms CharacterForm { get; set; }
        
        private Dictionary<CharacterForms, Vector2Int> m_slotsOnGrid;
        public abstract Vector2Int GetNextSize(Vector2 nextDirection);
        public abstract Vector2Int GetCurrentSize(CharacterForms currentForm);
        
        public abstract CharacterForms GetCurrentForm();
        
        protected abstract Tuple<CharacterForms, int> GetIndexAndForm(Vector2 direction, CharacterForms forms = CharacterForms.NORMAL);

        //protected abstract void OnCollisionEnter2D(Collision2D other);
    }
}
