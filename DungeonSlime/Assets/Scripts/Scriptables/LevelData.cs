using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Scriptables {

    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelDataObject", order = 2)]
    public class LevelData : ScriptableObject {
        
        [Header("Level infos")]
        public Texture2D LevelTexture;
        public int CurrentLevelData;
        public int NextLevelData;
        public TextAsset LevelJson;
    }
}