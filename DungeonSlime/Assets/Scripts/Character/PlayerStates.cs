using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Character {

    public class PlayerStates : MonoBehaviour {
        public enum SlimeForms {
            NORMAL,
            SEMI_STRETCHED_H,
            FULL_STRETCHED_H,
            SEMI_STRETCHED_V,
            FULL_STRETCHED_V
        }
        
        
        [SerializeField] private Animator m_anim;
        [SerializeField] private List<Sprite> m_slimeSprites;
        private SlimeForms m_slimeForm;
        
        private Dictionary <SlimeForms, Vector2Int> m_slotsOnGrid = new Dictionary<SlimeForms, Vector2Int> {
            {SlimeForms.NORMAL, new Vector2Int(4, 4)},
            {SlimeForms.SEMI_STRETCHED_H, new Vector2Int(6, 2)},
            {SlimeForms.FULL_STRETCHED_H, new Vector2Int(8, 1)},
            {SlimeForms.SEMI_STRETCHED_V,  new Vector2Int(2, 6)},
            {SlimeForms.FULL_STRETCHED_V, new Vector2Int(1, 8)},
        };
        
        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[(int)SlimeForms.NORMAL]));
            GameManager.Instance.GlobalDispatcher.Subscribe<OnFinishMovement>(OnFinishMovement);
            m_slimeForm = SlimeForms.NORMAL;
        }
        
        private void OnFinishMovement(OnFinishMovement ev) {
            var index = GetIndexForm(ev.CurrentDirection, m_slimeForm);
            m_slimeForm = (SlimeForms) index;
            m_anim.SetInteger("form", index);
            GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[index]));
        }
        
        public Vector2Int GetPlayerNextSize(Vector2 nextDirection) {
            var index = GetIndexForm(nextDirection);
            return GetCurrentSize((SlimeForms) index);
        }
        
        private int GetIndexForm(Vector2 direction, SlimeForms forms = SlimeForms.NORMAL) {
            var index = 0;
            
            switch (m_slimeForm) {
                case SlimeForms.NORMAL:
                    if (direction == Vector2.right || direction == Vector2.left) {
                        index = (int)SlimeForms.SEMI_STRETCHED_V;
                    } else if (direction == Vector2.up || direction == Vector2.down) {
                       index = (int)SlimeForms.SEMI_STRETCHED_H;
                    }
                    break;
                case SlimeForms.SEMI_STRETCHED_H:
                    if (direction == Vector2.right || direction == Vector2.left) {
                       index = (int)SlimeForms.NORMAL;
                    } else if (direction == Vector2.up || direction == Vector2.down) {
                       index = (int)SlimeForms.FULL_STRETCHED_H;
                    }
                    break;
                case SlimeForms.FULL_STRETCHED_H:
                    if (direction == Vector2.right || direction == Vector2.left) {
                        index = (int)SlimeForms.SEMI_STRETCHED_H;
                    } else if (direction == Vector2.up || direction == Vector2.down) {
                        index = (int)SlimeForms.FULL_STRETCHED_H;
                    }
                    break;
                case SlimeForms.SEMI_STRETCHED_V:
                    if (direction == Vector2.right || direction == Vector2.left) {
                        index = (int)SlimeForms.FULL_STRETCHED_V;
                    } else if (direction == Vector2.up || direction == Vector2.down) {
                        index = (int)SlimeForms.NORMAL;
                    }
                    break;
                case SlimeForms.FULL_STRETCHED_V:
                    if (direction == Vector2.right || direction == Vector2.left) {
                        index = (int)SlimeForms.FULL_STRETCHED_V;
                    } else if (direction == Vector2.up || direction == Vector2.down) {
                        index = (int)SlimeForms.SEMI_STRETCHED_V;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return index;
        }

        public Vector2Int GetCurrentSize(SlimeForms currentForm) {
            return m_slotsOnGrid[currentForm];
        }
        
        public SlimeForms GetCurrentForm() {
            return m_slimeForm;
        }
    }
}