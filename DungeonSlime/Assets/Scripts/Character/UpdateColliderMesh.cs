using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Character {

  public class UpdateColliderMesh : MonoBehaviour {

    public List<BoxCollider2D> colliders;
    private int m_currentColliderIndex = 0;
    
    private void Awake() {
      GameManager.Instance.GlobalDispatcher.Subscribe<OnUpdateSprite>(OnUpdateSprite);
    }

    private void OnUpdateSprite(OnUpdateSprite ev) {
      colliders[m_currentColliderIndex].enabled = false;
      m_currentColliderIndex = ev.SpriteColliderIndex;
      colliders[m_currentColliderIndex].enabled = true;
    }
  }
}