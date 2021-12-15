using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Managers;
using UnityEngine;

namespace DungeonSlime.Utils {

    [System.Serializable]
    public class UserData {

        public int lastLevelPlayed;
        public int normalLevelUnlocked = 1;
        public int hardLevelUnlocked = 1;
        public LevelManager.LevelDifficulty levelDifficulty;
        public bool isFullScreen;
        public bool isMusicOn;
        public bool isSfxOn;
    }
}