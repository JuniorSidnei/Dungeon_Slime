using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using DungeonSlime.Character;
using DungeonSlime.Managers;
using DungeonSlime.Utils;
using GameToBeNamed.Utils.Sound;
using UnityEngine;

namespace DungeonSlime.Enviroment {

    public class SpikeColliderMesh : ColliderMesh {
        public BoxCollider2D boxCollider;
        public Animator animator;
        public AudioClip deactiveSound;
        
        
        private void FixedUpdate() {
            if (!EnableCollision) return;
            
            var colliderBuffer = new Collider2D[5];
            var spriteBounds = SpriteRend.bounds;

            var sizeColliderBuffer = Physics2D.OverlapBoxNonAlloc(new Vector2(spriteBounds.center.x, spriteBounds.center.y), new Vector3(spriteBounds.size.x, spriteBounds.size.y, 0), 0, colliderBuffer, objectLayer);

            if (sizeColliderBuffer == 0) return;

            var objectId = colliderBuffer[0].gameObject.GetComponent<CharacterStates>().Id;
            
            GameManager.Instance.GlobalDispatcher.Emit(new OnCollisionWithSpikes(objectId));
        }

        public async void DeactiveSpike(bool enable) {
            animator.SetTrigger("deactive");
            await Task.Delay(500);
            AudioController.Instance.Play(deactiveSound, AudioController.SoundType.SoundEffect2D);
            EnableCollision = enable;
            boxCollider.enabled = enable;
        }
    }
}