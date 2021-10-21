using System;
using System.Collections.Generic;
using DungeonSlime.Enviroment;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Character {
    public class SlimeColliderMesh : MonoBehaviour {
      public LayerMask objectLayer;
      public SpriteRenderer spriteRenderer;
      private bool m_isColliding;
      private bool m_enableBox;
      private Collider2D m_boxCastResult;
      private CharacterMovement m_slimeObject;
      private int m_lasRockId;

      public bool IsPlayerMoving {
          set => m_enableBox = value;
      }

      public bool IsPlayerColliding {
          set => m_isColliding = value;
      }
      
      public int LastRockId {
          set => m_lasRockId = value;
      }
      
      private void Awake() {
          m_slimeObject =  gameObject.GetComponent<CharacterMovement>();
      }
      
      public void ValidateSlimeExpansion() {
          var colliderBuffer = new Collider2D[5];
          var spriteBounds = spriteRenderer.bounds;

          var sizeColliderBuffer = Physics2D.OverlapBoxNonAlloc(new Vector2(spriteBounds.center.x, spriteBounds.center.y), new Vector3(spriteBounds.size.x, spriteBounds.size.y, 0), 0, colliderBuffer, objectLayer);

          if (sizeColliderBuffer == 0) return;
          
          for (var i = 0; i < sizeColliderBuffer; i++) {
              var objectCollider = colliderBuffer[i].gameObject.GetComponent<RockStates>();
              
              if (objectCollider.Id == m_lasRockId || objectCollider.characterMovement.IsMoving) return;
              
              var directionToMove = objectCollider.GetAxisToMove(m_slimeObject.CurrentFinalPosition, m_slimeObject.CurrentDirection);
              var rockShouldBeDestroyed = RockCanMoveWithinDirection(objectCollider, directionToMove);
              objectCollider.MoveToDestination(m_slimeObject.CurrentFinalPosition, directionToMove, m_slimeObject.CurrentSize, !rockShouldBeDestroyed);
          }
      }
      
      private void FixedUpdate() {
          if (!m_enableBox) { return; }
          
          var colliderBuffer = new Collider2D[5];
          var boxResultSize = CreateBoxCastWithinDirection(m_slimeObject.CurrentDirection, spriteRenderer, colliderBuffer);
          if (boxResultSize == 0 || m_isColliding) return;

          m_isColliding = true;
          for (var i = 0; i < boxResultSize; i++) {
              var objectCollider = colliderBuffer[i].gameObject.GetComponent<RockStates>();
              var collisionPosition = objectCollider.GetPivotPosition(m_slimeObject.CurrentDirection);

              m_lasRockId = objectCollider.Id;
              GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterCollision(m_lasRockId, collisionPosition, RockCanMoveWithinDirection(objectCollider, m_slimeObject.CurrentDirection)));
          }
      }
      
      public bool CanIMoveWithinDirection(Vector2Int direction) {
          Collider2D[] collidersBuffer = new Collider2D[5];
          var sizeColliderBuffer = CreateBoxCastWithinDirection(direction, spriteRenderer, collidersBuffer);
          return sizeColliderBuffer == 0;
      }
      
      private int CreateBoxCastWithinDirection(Vector2Int direction, Renderer sprite, Collider2D[] colliderBuffer) {
          if (direction == Vector2Int.zero) return 0;
          
          var spriteBounds = sprite.bounds;
          var spriteSize = spriteBounds.size;
          var boxSize = Vector2.zero;
          var boxOrigin = Vector2.zero;

         
          if (direction == Vector2Int.left) {
              boxSize = m_slimeObject.CurrentSize.x == 12 ? new Vector2(0.1f, spriteSize.y - 0.3f) : new Vector2(0.1f, spriteSize.y);
              boxOrigin = new Vector2(spriteBounds.min.x, spriteBounds.max.y - spriteSize.y / 2);
          }
          else if (direction == Vector2Int.right) {
              boxSize =  m_slimeObject.CurrentSize.x == 12 ? new Vector2(0.1f, spriteSize.y - 0.3f) : new Vector2(0.1f, spriteSize.y);
              boxOrigin = new Vector2(spriteBounds.max.x, spriteBounds.max.y - spriteSize.y / 2);
          }
          else if (direction == Vector2Int.down) {
              boxSize = m_slimeObject.CurrentSize.y == 12 ? new Vector2(0.01f, 0.2f) : new Vector2(spriteSize.x, 0.1f);
              boxOrigin = new Vector2(spriteBounds.max.x - spriteSize.x / 2, spriteBounds.min.y);
          }
          else if (direction == Vector2Int.up) {
              //just to test with the wrong sprite
              boxSize = m_slimeObject.CurrentSize.y == 12 ? new Vector2(0.01f, 0.2f) : new Vector2(spriteSize.x, 0.2f);
              boxOrigin = new Vector2(spriteBounds.max.x - spriteSize.x / 2, spriteBounds.max.y);
          }
          
          var sizeColliderBuffer = Physics2D.OverlapBoxNonAlloc(boxOrigin, boxSize, 0, colliderBuffer, objectLayer);
          return sizeColliderBuffer;
      }
      
      private bool RockCanMoveWithinDirection(RockStates rock, Vector2Int direction) {
          if (direction == Vector2Int.zero) return false;
          
          var offsetValueX = 0.35f;
          var offsetValueY = 0.01f;
          
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
          
          RaycastHit2D hit = Physics2D.Raycast(rockPos, direction, 0.0f, objectLayer);
          //Debug.Log("quem? " + hit.collider.gameObject.name);
          Debug.DrawRay(new Vector2(rockPos.x, rockPos.y), new Vector3(direction.x, direction.y, 0), Color.green, 2f);
          return !hit;
      }
      
      void OnDrawGizmos()  {
        Gizmos.color = new Color(1, 1, 0, 0.5f);

        var spriteBounds = spriteRenderer.bounds;
        //Gizmos.DrawCube(new Vector2(spriteBorder.x, spriteBorder.y  - (spriteRenderer.bounds.size.y/2)), new Vector3(0.1f, spriteRenderer.bounds.size.y, 0));
        //Gizmos.DrawCube(new Vector2(spriteRenderer.bounds.center.x, spriteBorder.y  - (spriteRenderer.bounds.size.y/2)), new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 0));
        //Gizmos.DrawCube(new Vector2(spriteRenderer.bounds.center.x, spriteRenderer.bounds.center.y), new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 0));
        //Gizmos.DrawCube(new Vector2(spriteBorder.x - (spriteRenderer.bounds.size.x/2), spriteRenderer.bounds.center.y), new Vector3(spriteRenderer.bounds.size.x, 0.2f, 0));
        //Gizmos.DrawCube(new Vector2(spriteBorder.x - (sprite.bounds.size.x/2), sprite.bounds.max.y), new Vector3(sprite.bounds.size.x, 0.1f, 0));
        
        
        //Gizmos.DrawWireCube(new Vector2(spriteRenderer.bounds.center.x, spriteRenderer.bounds.center.y), new Vector3(0.05f, 0.1f, 0));
//        Gizmos.DrawWireCube(new Vector2(spriteBounds.max.x, spriteBounds.max.y - spriteBounds.size.y / 2),
//            new Vector2(0.1f, spriteBounds.size.y - 0.3f));
          

        Gizmos.DrawWireCube(new Vector2(spriteBounds.center.x, spriteBounds.center.y),
            new Vector2(spriteBounds.size.x, spriteBounds.size.y));

          if (!m_enableBox) return;
          var spriteSize = spriteRenderer.bounds.center;
        
       
        //Gizmos.DrawCube(new Vector2(spriteSize.x, spriteSize.y), new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 0));
      }
    }
}