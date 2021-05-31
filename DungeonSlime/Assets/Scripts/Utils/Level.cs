using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    [System.Serializable]
    public class Level {
        public Block[] blocks;
        public int columnCount = 5;
    }
}