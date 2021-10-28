using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Scriptables {

    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelDataObject", order = 2)]
    public class LevelData : ScriptableObject {
        
        [Header("Level infos")]
        public Texture2D levelTexture;
        public int currentLevelData;
        public int nextLevelData;
        public int columnCount;
        public int numberOfMovements;
        public TextAsset levelJson;
    }
}