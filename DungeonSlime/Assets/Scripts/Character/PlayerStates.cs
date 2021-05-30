using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Character {

    public class PlayerStates : MonoBehaviour {

        enum SlimeForms {
            NORMAL,
            SEMI_STRECHED_H,
            FULL_STRETCHED_H,
            SEMI_STRETCHED_V,
            FULL_STRETCHED_V
        }
        
        [SerializeField] private Animator m_anim;
        private SlimeForms m_slimeForms;
        
        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnCollisionDetected>(OnCollisionDetected);
            m_slimeForms = SlimeForms.NORMAL;
        }

        private void Update() {
            switch (m_slimeForms) {
                case SlimeForms.NORMAL:
                    m_anim.SetTrigger("normal");
                    break;
                case SlimeForms.SEMI_STRECHED_H:
                    m_anim.SetTrigger("semi_stretched_h");
                    break;
                case SlimeForms.FULL_STRETCHED_H:
                    m_anim.SetTrigger("full_stretched_h");
                    break;
                case SlimeForms.SEMI_STRETCHED_V:
                    m_anim.SetTrigger("semi_stretched_v");
                    break;
                case SlimeForms.FULL_STRETCHED_V:
                    m_anim.SetTrigger("full_stretched_v");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnCollisionDetected(OnCollisionDetected ev) {
            switch (m_slimeForms) {
                case SlimeForms.NORMAL:
                    if (ev.CurrentDirection == Vector2.right || ev.CurrentDirection == Vector2.left) {
                        m_slimeForms = SlimeForms.SEMI_STRETCHED_V;
                    } else if (ev.CurrentDirection == Vector2.up || ev.CurrentDirection == Vector2.down) {
                        m_slimeForms = SlimeForms.SEMI_STRECHED_H;
                    }
                    break;
                case SlimeForms.SEMI_STRECHED_H:
                    if (ev.CurrentDirection == Vector2.right || ev.CurrentDirection == Vector2.left) {
                        m_slimeForms = SlimeForms.NORMAL;
                    } else if (ev.CurrentDirection == Vector2.up || ev.CurrentDirection == Vector2.down) {
                        m_slimeForms = SlimeForms.FULL_STRETCHED_H;
                    }
                    break;
                case SlimeForms.FULL_STRETCHED_H:
                    if (ev.CurrentDirection == Vector2.right || ev.CurrentDirection == Vector2.left) {
                        m_slimeForms = SlimeForms.SEMI_STRECHED_H;
                    }
                    break;
                case SlimeForms.SEMI_STRETCHED_V:
                    if (ev.CurrentDirection == Vector2.right || ev.CurrentDirection == Vector2.left) {
                        m_slimeForms = SlimeForms.FULL_STRETCHED_V;
                    } else if (ev.CurrentDirection == Vector2.up || ev.CurrentDirection == Vector2.down) {
                        m_slimeForms = SlimeForms.NORMAL;
                    }
                    break;
                case SlimeForms.FULL_STRETCHED_V:
                    if (ev.CurrentDirection == Vector2.up || ev.CurrentDirection == Vector2.down) {
                        m_slimeForms = SlimeForms.SEMI_STRETCHED_V;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ev.OnFinishAnimation?.Invoke();
        }
    }
}