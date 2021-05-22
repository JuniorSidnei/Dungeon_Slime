using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Character  {
	public class Controller2D : RaycastController2D  {
		public float maxSlopeAngle = 80;

		public CollisionInfo collisions;

		public override void Start() {
			base.Start();
		}

		public void Move(Vector2 moveAmount, bool standingOnPlatform = false) {
			
			UpdateRaycastOrigins();
			collisions.Reset();
			//collisions.moveAmountOld = moveAmount;

			if (moveAmount.x != 0) {
				collisions.faceHor = (int) Mathf.Sign(moveAmount.x);
			}
			
			HorizontalCollisions(ref moveAmount);
			
			if (moveAmount.y != 0) {
				collisions.faceVert = (int) Mathf.Sign(moveAmount.y);
				VerticalCollisions(ref moveAmount);
			}
			
			transform.Translate(moveAmount);
		}

		void HorizontalCollisions(ref Vector2 moveAmount)
		{
			float directionX = collisions.faceHor;
//			float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
//			
//			if (Mathf.Abs(moveAmount.x) < skinWidth)  {
//				rayLength = 2 * skinWidth;
//			}

			float rayLength = 2 * skinWidth;
			
			for (int i = 0; i < horizontalRayCount; i++)  {
				Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

				Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

				if (hit)  {
					
					if (hit.distance == 0)  {
						continue;
					}
					
					moveAmount.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;
					
					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}
			}
		}

		void VerticalCollisions(ref Vector2 moveAmount) {
			
			float directionY = collisions.faceVert;
			//float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;
			float rayLength = rayLength = 2 * skinWidth;
			
//			if (Mathf.Abs(moveAmount.y) < skinWidth) {
//				rayLength = 2 * skinWidth;
//			}
			
			for (int i = 0; i < verticalRayCount; i++)  {

				Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

				Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

				if (hit)  {
					if (hit.distance == 0)  {
						continue;
					}
					
					moveAmount.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;
					
					collisions.below = directionY == -1;
					collisions.above = directionY == 1;
				}
			}

		}
		
		public struct CollisionInfo
		{
			public bool above, below;
			public bool left, right;
			
			public int faceHor, faceVert;

			public void Reset()  {
				above = below = false;
				left = right = false;
			}
		}
	}
}