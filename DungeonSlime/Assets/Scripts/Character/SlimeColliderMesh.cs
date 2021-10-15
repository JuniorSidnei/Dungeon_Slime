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
      
      private void Awake() {
          GameManager.Instance.GlobalDispatcher.Subscribe<OnFinishMovement>(OnFinishMovement);
          m_slimeObject =  gameObject.GetComponent<CharacterMovement>();
      }

      private void OnFinishMovement(OnFinishMovement ev) {
          m_isColliding = false;
          m_enableBox = false;
      }

      public void ValidateSlimeExpansion() {
          var boxCast = new Collider2D();
          var spriteBounds = spriteRenderer.bounds;
          boxCast = Physics2D.OverlapBox(new Vector2(spriteBounds.center.x, spriteBounds.center.y), new Vector3(spriteBounds.size.x, spriteBounds.size.y, 0), objectLayer);
          
          if (boxCast == null) return;
          
          var objectCollider = boxCast.gameObject.GetComponent<RockStates>();

          if (objectCollider.Id == m_lasRockId) return;
          
          objectCollider.MoveToDestination(m_slimeObject.CurrentPosition, m_slimeObject.CurrentDirection, m_slimeObject.CurrentSize);
      }
      
      private void FixedUpdate() {
          if (!m_enableBox) return;
          
          var boxResult = CreateBoxCastWithinDirection(m_slimeObject.CurrentDirection, spriteRenderer);
          
          if (boxResult.collider == null || m_isColliding) return;
          
          m_isColliding = true;
          var objectCollider = boxResult.collider.gameObject.GetComponent<RockStates>();
          var collisionPosition = objectCollider.GetPivotPosition(m_slimeObject.CurrentDirection);
          m_lasRockId = objectCollider.Id;
          GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterCollision(objectCollider.Id, collisionPosition, RockCanMoveWithinDirection(objectCollider, m_slimeObject.CurrentDirection)));
      }
      
      public bool CanIMoveWithinDirection(Vector2Int direction) {
          return !CreateBoxCastWithinDirection(direction, spriteRenderer);
      }
      
      private RaycastHit2D CreateBoxCastWithinDirection(Vector2Int direction, SpriteRenderer sprite) {
          if (direction == Vector2Int.zero) return new RaycastHit2D();
          
          var spriteBounds = sprite.bounds;
          var spriteSize = spriteBounds.size;
          var boxSize = Vector2.zero;
          var boxOrigin = Vector2.zero;

         
          if (direction == Vector2Int.left) {
              boxSize = new Vector2(0.1f, spriteSize.y);
              boxOrigin = new Vector2(spriteBounds.min.x, spriteBounds.max.y - spriteSize.y / 2);
          }
          else if (direction == Vector2Int.right) {
              boxSize = new Vector2(0.1f, spriteSize.y);
              boxOrigin = new Vector2(spriteBounds.max.x, spriteBounds.max.y - spriteSize.y / 2);
          }
          else if (direction == Vector2Int.down) {
              boxSize = new Vector2(spriteSize.x, 0.1f);
              boxOrigin = new Vector2(spriteBounds.max.x - spriteSize.x / 2, spriteBounds.min.y);
          }
          else if (direction == Vector2Int.up) {
              boxSize = new Vector2(spriteSize.x, 0.1f);
              boxOrigin = new Vector2(spriteBounds.max.x - spriteSize.x / 2, spriteBounds.max.y);
          }

          var boxResult = Physics2D.BoxCast(boxOrigin, boxSize, 0f, direction, 0.1f, objectLayer);
          return boxResult;
      }
      
      private bool RockCanMoveWithinDirection(RockStates rock, Vector2Int direction) {
          if (direction == Vector2Int.zero) return false;
          
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
      
      void OnDrawGizmos()  {
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        var spriteBorder = spriteRenderer.bounds.max;
        //Gizmos.DrawCube(new Vector2(spriteBorder.x, spriteBorder.y  - (spriteRenderer.bounds.size.y/2)), new Vector3(0.1f, spriteRenderer.bounds.size.y, 0));
        //Gizmos.DrawCube(new Vector2(spriteRenderer.bounds.center.x, spriteBorder.y  - (spriteRenderer.bounds.size.y/2)), new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 0));
        //Gizmos.DrawCube(new Vector2(spriteRenderer.bounds.center.x, spriteRenderer.bounds.center.y), new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 0));
        //Gizmos.DrawCube(new Vector2(spriteBorder.x - (spriteRenderer.bounds.size.x/2), spriteRenderer.bounds.center.y), new Vector3(spriteRenderer.bounds.size.x, 0.2f, 0));
        //Gizmos.DrawCube(new Vector2(spriteBorder.x - (sprite.bounds.size.x/2), sprite.bounds.max.y), new Vector3(sprite.bounds.size.x, 0.1f, 0));
        
        if (!m_enableBox) return;
        var spriteSize = spriteRenderer.bounds.center;
        Gizmos.DrawCube(new Vector2(spriteSize.x, spriteSize.y), new Vector3(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y, 0));
      }
    }
}