using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Character {

  public class UpdateColliderMesh : MonoBehaviour {
    public LayerMask objectLayer;
    public SpriteRenderer sprite;

    private void FixedUpdate() {
      var spriteSize = sprite.bounds.size;
      var spriteCenter = sprite.bounds.center;
      var boxResult = Physics2D.BoxCast(new Vector2(spriteCenter.x, spriteCenter.y), new Vector2(spriteSize.x, spriteSize.y), 0f, Vector2.right, 0.1f, objectLayer);

      if (boxResult.collider != null) {
          Debug.Log("Bati");
      }
    }
//    void OnDrawGizmos()  {
//      Gizmos.color = new Color(1, 0, 0, 0.5f);
//      var spriteSize = sprite.bounds.center;
//      Gizmos.DrawCube(new Vector2(spriteSize.x, spriteSize.y), new Vector3(sprite.bounds.size.x, sprite.bounds.size.y, 0));
//    }
  }
}