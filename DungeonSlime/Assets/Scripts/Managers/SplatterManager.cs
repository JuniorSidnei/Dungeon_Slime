using System.Collections;
using System.Collections.Generic;
using GameToBeNamed.Utils;
using UnityEngine;

namespace DungeonSlime.Managers {

    public class SplatterManager : Singleton<SplatterManager> {

        public GameObject splatter;
        private int m_sortingIndex = 0;
        
        public void CreateSplatter(Vector3 position, Vector2 slimeSize, float rotation) {
            var splatterRenderer = Instantiate(splatter, position, Quaternion.Euler(new Vector3(0, 0, rotation)), transform).GetComponent<SpriteRenderer>();

            splatter.transform.localScale = slimeSize;
            splatterRenderer.sortingOrder = m_sortingIndex;
            m_sortingIndex++;
        }
    }
}
