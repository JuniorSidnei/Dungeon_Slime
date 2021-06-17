using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Character {

    public class PlayerStates : MonoBehaviour {

        enum SlimeForms {
            NORMAL,
            SEMI_STRETCHED_H,
            FULL_STRETCHED_H,
            SEMI_STRETCHED_V,
            FULL_STRETCHED_V
        }
        
        
        [SerializeField] private Animator m_anim;
        [SerializeField] private List<Sprite> m_slimeSprites;
        private SlimeForms m_slimeForms;
        
        private Dictionary <SlimeForms, int> m_slotsOnGridX = new Dictionary<SlimeForms, int> {
            {SlimeForms.NORMAL, 1},
            {SlimeForms.SEMI_STRETCHED_H, 1},
            {SlimeForms.FULL_STRETCHED_H, 1},
        };
        
        private Dictionary <SlimeForms, int> m_slotsOnGridY = new Dictionary<SlimeForms, int> {
            {SlimeForms.NORMAL, 3},
            {SlimeForms.SEMI_STRETCHED_V, 2},
            {SlimeForms.FULL_STRETCHED_V, 1},
        };
        
        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[(int)SlimeForms.NORMAL]));
            GameManager.Instance.GlobalDispatcher.Subscribe<OnFinishMovement>(OnFinishMovement);
            m_slimeForms = SlimeForms.NORMAL;
        }
        
        private void OnFinishMovement(OnFinishMovement ev) {
            var index = GetIndexForm(ev.CurrentDirection, m_slimeForms);
            m_slimeForms = (SlimeForms) index;
            m_anim.SetInteger("form", index);
            GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[index]));
            
//            switch (m_slimeForms) {
//                case SlimeForms.NORMAL:
//                    if (ev.CurrentDirection == Vector2.right || ev.CurrentDirection == Vector2.left) {
//                        m_slimeForms = SlimeForms.SEMI_STRETCHED_V;
//                        m_anim.SetTrigger("semi_stretched_v");
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[(int)SlimeForms.SEMI_STRETCHED_V]));
//                    } else if (ev.CurrentDirection == Vector2.up || ev.CurrentDirection == Vector2.down) {
//                        m_slimeForms = SlimeForms.SEMI_STRETCHED_H;
//                        m_anim.SetTrigger("semi_stretched_h");
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[(int)SlimeForms.SEMI_STRETCHED_H]));
//                    }
//                    break;
//                case SlimeForms.SEMI_STRETCHED_H:
//                    if (ev.CurrentDirection == Vector2.right || ev.CurrentDirection == Vector2.left) {
//                        m_slimeForms = SlimeForms.NORMAL;
//                        m_anim.SetTrigger("normal");
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[(int)SlimeForms.NORMAL]));
//                    } else if (ev.CurrentDirection == Vector2.up || ev.CurrentDirection == Vector2.down) {
//                        m_slimeForms = SlimeForms.FULL_STRETCHED_H;
//                        m_anim.SetTrigger("full_stretched_h");
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[(int)SlimeForms.FULL_STRETCHED_H]));
//                    }
//                    break;
//                case SlimeForms.FULL_STRETCHED_H:
//                    if (ev.CurrentDirection == Vector2.right || ev.CurrentDirection == Vector2.left) {
//                        m_slimeForms = SlimeForms.SEMI_STRETCHED_H;
//                        m_anim.SetTrigger("semi_stretched_h");
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[(int)SlimeForms.SEMI_STRETCHED_H]));
//                    }
//                    break;
//                case SlimeForms.SEMI_STRETCHED_V:
//                    if (ev.CurrentDirection == Vector2.right || ev.CurrentDirection == Vector2.left) {
//                        m_slimeForms = SlimeForms.FULL_STRETCHED_V;
//                        m_anim.SetTrigger("full_stretched_v");
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[(int)SlimeForms.FULL_STRETCHED_V]));
//                    } else if (ev.CurrentDirection == Vector2.up || ev.CurrentDirection == Vector2.down) {
//                        m_slimeForms = SlimeForms.NORMAL;
//                        m_anim.SetTrigger("normal");
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[(int)SlimeForms.NORMAL]));
//                    }
//                    break;
//                case SlimeForms.FULL_STRETCHED_V:
//                    if (ev.CurrentDirection == Vector2.up || ev.CurrentDirection == Vector2.down) {
//                        m_slimeForms = SlimeForms.SEMI_STRETCHED_V;
//                        m_anim.SetTrigger("semi_stretched_v");
//                        GameManager.Instance.GlobalDispatcher.Emit(new OnUpdateSprite(m_slimeSprites[(int)SlimeForms.SEMI_STRETCHED_V]));
//                    }
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
        }

        private int GetIndexForm(Vector2 direction, SlimeForms forms = SlimeForms.NORMAL) {
            var index = 0;
            
            switch (m_slimeForms) {
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
        
        public Sprite GetNextSpriteToMovement(Vector2 nextDirection) {
            var index = GetIndexForm(nextDirection);
            return m_slimeSprites[index];

//            var index = 0;
//            
//            switch (m_slimeForms) {
//                case SlimeForms.NORMAL:
//                    if (nextDirection == Vector2.right || nextDirection == Vector2.left) {
//                        index = (int)SlimeForms.SEMI_STRETCHED_V;
//                    } else if (nextDirection == Vector2.up || nextDirection == Vector2.down) {
//                       index = (int)SlimeForms.SEMI_STRETCHED_H;
//                    }
//                    break;
//                case SlimeForms.SEMI_STRETCHED_H:
//                    if (nextDirection == Vector2.right || nextDirection == Vector2.left) {
//                       index = (int)SlimeForms.NORMAL;
//                    } else if (nextDirection == Vector2.up || nextDirection == Vector2.down) {
//                       index = (int)SlimeForms.FULL_STRETCHED_H;
//                    }
//                    break;
//                case SlimeForms.FULL_STRETCHED_H:
//                    if (nextDirection == Vector2.right || nextDirection == Vector2.left) {
//                        index = (int)SlimeForms.SEMI_STRETCHED_H;
//                    } else if (nextDirection == Vector2.up || nextDirection == Vector2.down) {
//                        index = (int)SlimeForms.FULL_STRETCHED_H;
//                    }
//                    break;
//                case SlimeForms.SEMI_STRETCHED_V:
//                    if (nextDirection == Vector2.right || nextDirection == Vector2.left) {
//                        index = (int)SlimeForms.FULL_STRETCHED_V;
//                    } else if (nextDirection == Vector2.up || nextDirection == Vector2.down) {
//                        index = (int)SlimeForms.NORMAL;
//                    }
//                    break;
//                case SlimeForms.FULL_STRETCHED_V:
//                    if (nextDirection == Vector2.right || nextDirection == Vector2.left) {
//                        index = (int)SlimeForms.FULL_STRETCHED_V;
//                    } else if (nextDirection == Vector2.up || nextDirection == Vector2.down) {
//                        index = (int)SlimeForms.SEMI_STRETCHED_V;
//                    }
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//
//            return m_slimeSprites[index];
        }
        
        
    }
}