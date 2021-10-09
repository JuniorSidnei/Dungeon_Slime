using System;
using DungeonSlime.Enviroment;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Character {
    public class UpdateColliderMesh : MonoBehaviour {
      public LayerMask objectLayer;
      public SpriteRenderer sprite;
      private bool m_isColliding;

      private void Awake() {
          GameManager.Instance.GlobalDispatcher.Subscribe<OnFinishMovement>(OnFinishMovement);
      }

      private void OnFinishMovement(OnFinishMovement ev) {
          m_isColliding = false;
      }
      
      private void FixedUpdate() {
        var spriteSize = sprite.bounds.size;
        var spriteCenter = sprite.bounds.center;
        var boxResult = Physics2D.BoxCast(new Vector2(spriteCenter.x, spriteCenter.y), new Vector2(spriteSize.x, spriteSize.y), 0f, Vector2.right, 0.1f, objectLayer);

        if (boxResult.collider == null || m_isColliding) return;
        
        m_isColliding = true;
        var objectCollider = boxResult.collider.gameObject.GetComponent<RockStates>();
        var currentDirection =  gameObject.GetComponent<CharacterMovement>().CurrentDirection;
        var collisionPosition = objectCollider.GetPivotPosition(currentDirection);
        GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterCollision(objectCollider.Id, collisionPosition));
        
      }
      
  //    void OnDrawGizmos()  {
  //      Gizmos.color = new Color(1, 0, 0, 0.5f);
  //      var spriteSize = sprite.bounds.center;
  //      Gizmos.DrawCube(new Vector2(spriteSize.x, spriteSize.y), new Vector3(sprite.bounds.size.x, sprite.bounds.size.y, 0));
  //    }
    }
}