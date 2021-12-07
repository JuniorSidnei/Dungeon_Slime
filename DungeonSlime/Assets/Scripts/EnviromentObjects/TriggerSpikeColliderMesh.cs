using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Enviroment;
using GameToBeNamed.Utils.Sound;
using TMPro;
using UnityEngine;

public class TriggerSpikeColliderMesh : ColliderMesh {

    public SpikeColliderMesh spikeColliderMesh;
    public AudioClip StoneSetted;
    
    private void FixedUpdate() {
        if (!EnableCollision) return;
        
        var colliderBuffer = new Collider2D[5];
        var spriteBounds = SpriteRend.bounds;

        var sizeColliderBuffer = Physics2D.OverlapBoxNonAlloc(new Vector2(spriteBounds.center.x, spriteBounds.center.y), new Vector3(spriteBounds.size.x, spriteBounds.size.y, 0), 0, colliderBuffer, objectLayer);

        if (sizeColliderBuffer == 0) return;

        AudioController.Instance.Play(StoneSetted, AudioController.SoundType.SoundEffect2D);
        //spikeColliderMesh.SetCollisionEnabled(false);
        spikeColliderMesh.DeactiveSpike(false);
        EnableCollision = false;
    }
}
