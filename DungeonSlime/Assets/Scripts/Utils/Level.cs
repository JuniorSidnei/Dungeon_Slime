using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    [System.Serializable]
    public class Level {
        public Block[] blocks;
        public int columnCount = 20;

        public Block getBlock(Vector2Int position) {
            return blocks[position.x + position.y * columnCount];
        }
    }
}