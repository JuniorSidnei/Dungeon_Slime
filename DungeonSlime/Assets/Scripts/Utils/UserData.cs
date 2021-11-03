using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Utils {

    [System.Serializable]
    public class UserData {

        public int lastLevelPlayed;
        public int normalLevelUnlocked = 1;
        public int hardLevelUnlocked = 1;
        public int levelDifficulty;
        public bool isFullScreen;
        public bool isMusicOn;
        public bool isSfxOn;
    }
}