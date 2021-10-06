using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    [System.Serializable]
    public class Block {
        public enum BlockType {
            Floor = 0,
            Wall = 1,
            Empty = 2,
            InitialPosition = 3,
            Endgame = 4,
            Spikes = 5,
            BlockWall = 6,
            FakeSpikes = 7
        }
        
        public BlockType type;
    }
}