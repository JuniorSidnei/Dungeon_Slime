using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    [System.Serializable]
    public class Block {
        public enum BlockType {
            Empty = 0,
            Wall = 1
        }
        
        public BlockType type;
    }
}