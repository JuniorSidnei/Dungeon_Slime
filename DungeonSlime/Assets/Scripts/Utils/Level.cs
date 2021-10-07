using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    [System.Serializable]
    public class Level {
        public Block[] blocks;
        public int columnCount = 89;
        public int height;
        public int width;

        public Block GetBlock(Vector2Int position) {
            return blocks[position.x + position.y * (columnCount)];
        }
    }
}