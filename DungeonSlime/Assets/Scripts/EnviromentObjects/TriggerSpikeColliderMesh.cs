using System;
using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Enviroment;
using TMPro;
using UnityEngine;

public class TriggerSpikeColliderMesh : ColliderMesh {

    public SpikeColliderMesh spikeColliderMesh;
    private void FixedUpdate() {
        if (!EnableCollision) return;
        
        var colliderBuffer = new Collider2D[5];
        var spriteBounds = SpriteRend.bounds;

        var sizeColliderBuffer = Physics2D.OverlapBoxNonAlloc(new Vector2(spriteBounds.center.x, spriteBounds.center.y), new Vector3(spriteBounds.size.x, spriteBounds.size.y, 0), 0, colliderBuffer, objectLayer);

        if (sizeColliderBuffer == 0) return;
        
        spikeColliderMesh.SetCollisionEnabled(false);
        EnableCollision = false;
    }
}
