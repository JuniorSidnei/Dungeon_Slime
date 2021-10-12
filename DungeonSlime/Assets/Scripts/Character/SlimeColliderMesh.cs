using System;
using DungeonSlime.Enviroment;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Character {
    public class SlimeColliderMesh : MonoBehaviour {
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
        var slimeObject =  gameObject.GetComponent<CharacterMovement>();
        
        var collisionPosition = objectCollider.GetPivotPosition(slimeObject.CurrentDirection, slimeObject.CurrentSize);
        GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterCollision(objectCollider.Id, collisionPosition, CanIMoveWithDirection(objectCollider, slimeObject.CurrentDirection)));
      }

      private bool CanIMoveWithDirection(RockStates rock, Vector2Int direction) {
          var offsetValueX = 0.35f;
          var offsetValueY = 0.15f;
          
          if (direction == Vector2Int.left) {
              offsetValueX *= -1;
          } else if (direction == Vector2Int.up) {
              offsetValueX = 0;
              offsetValueY = 0.35f;
          } else if (direction == Vector2Int.down) {
              offsetValueX = 0;
              offsetValueY = -0.35f;
          }

          var basePos = rock.gameObject.transform.position;
          var rockPos = new Vector2(basePos.x + offsetValueX, basePos.y + offsetValueY);
          
          RaycastHit2D hit = Physics2D.Raycast(rockPos, direction, 0.1f, objectLayer);
          //Debug.DrawRay(new Vector2(rockPos.x + 0.35f, rockPos.y + 0.15f), Vector3.right, Color.green, 10f);
          return !hit;
          
          //Debug.Log("quem? " + hit.collider.gameObject.name);
      }
      
//      void OnDrawGizmos()  {
//        Gizmos.color = new Color(1, 0, 0, 0.5f);
//        var spriteSize = sprite.bounds.center;
//        Gizmos.DrawCube(new Vector2(spriteSize.x, spriteSize.y), new Vector3(sprite.bounds.size.x, sprite.bounds.size.y, 0));
//      }
    }
}