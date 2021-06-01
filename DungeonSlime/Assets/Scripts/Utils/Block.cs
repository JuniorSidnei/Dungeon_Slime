using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    [System.Serializable]
    public class Block {
        public enum BlockType {
            Floor = 0,
            Wall = 1,
            Black = 2
        }
        
        public BlockType type;
    }
}