using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Character {

    public class HandleSlimeExpansion : MonoBehaviour {

        public SlimeColliderMesh slimeColliderMesh;
        
        public void CreateOverlapBoxWithSprite() {
            if (gameObject.GetComponentInParent<CharacterMovement>().IsMoving) return;
            
            slimeColliderMesh.ValidateSlimeExpansion();    
        }
    }
}