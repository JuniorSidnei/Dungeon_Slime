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
      private bool m_enableBox;

      public bool IsPlayerMoving {
          set => m_enableBox = value;
      }
      
      private void Awake() {
          GameManager.Instance.GlobalDispatcher.Subscribe<OnFinishMovement>(OnFinishMovement);
      }

      
      private void OnFinishMovement(OnFinishMovement ev) {
          m_isColliding = false;
          m_enableBox = false;
      }
      
      private void FixedUpdate() {
          if (!m_enableBox) return;
          
          var spriteSize = sprite.bounds.size;
          var spriteCenter = sprite.bounds.center;
          var boxResult = Physics2D.BoxCast(new Vector2(spriteCenter.x, spriteCenter.y), new Vector2(spriteSize.x, spriteSize.y), 0f, Vector2.right, 0.1f, objectLayer);   
            
          if (boxResult.collider == null || m_isColliding) return;
        
           m_isColliding = true;
           var objectCollider = boxResult.collider.gameObject.GetComponent<RockStates>();
           var slimeObject =  gameObject.GetComponent<CharacterMovement>();
           var collisionPosition = objectCollider.GetPivotPosition(slimeObject.CurrentDirection);
           
           GameManager.Instance.GlobalDispatcher.Emit(new OnCharacterCollision(objectCollider.Id, collisionPosition, RockCanMoveWithinDirection(objectCollider, slimeObject.CurrentDirection)));
      }
      
      public bool CanIMoveWithinDirection(Vector2Int direction) {
          var spriteSize = sprite.bounds.size;
          var spriteBounds = sprite.bounds;
          Vector2 boxSize;
          Vector2 boxOrigin;
          
          if (direction == Vector2Int.left) {
              boxSize = new Vector2(0.1f, spriteSize.y); 
              boxOrigin = new Vector2(spriteBounds.min.x, spriteBounds.max.y  - sprite.bounds.size.y/2);
          } else if (direction == Vector2Int.right) {
              boxSize = new Vector2(0.1f, spriteSize.y); 
              boxOrigin = new Vector2(spriteBounds.max.x, spriteBounds.max.y  - sprite.bounds.size.y/2);
          }
          else if(direction == Vector2Int.down){
              boxSize = new Vector2(spriteSize.x, 0.1f);
              boxOrigin = new Vector2(spriteBounds.max.x - sprite.bounds.size.x/2, spriteBounds.max.y);
          } else {
              boxSize = new Vector2(spriteSize.x, 0.1f);
              boxOrigin = new Vector2(spriteBounds.max.x - sprite.bounds.size.x/2, spriteBounds.min.y);
          }

          var boxResult = Physics2D.BoxCast(boxOrigin, boxSize, 0f, direction, 0.01f, objectLayer);
          return !boxResult;
      }
      
      private bool RockCanMoveWithinDirection(RockStates rock, Vector2Int direction) {
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

        var spriteBorder = sprite.bounds.max;
        //Gizmos.DrawCube(new Vector2(spriteBorder.x, spriteBorder.y  - (sprite.bounds.size.y/2)), new Vector3(0.1f, sprite.bounds.size.y, 0));
        //Gizmos.DrawCube(new Vector2(spriteBorder.x - (sprite.bounds.size.x/2), sprite.bounds.min.y), new Vector3(sprite.bounds.size.x, 0.1f, 0));
        
        if (!m_enableBox) return;
        var spriteSize = sprite.bounds.center;
        Gizmos.DrawCube(new Vector2(spriteSize.x, spriteSize.y), new Vector3(sprite.bounds.size.x, sprite.bounds.size.y, 0));

        
        
      }
    }
}