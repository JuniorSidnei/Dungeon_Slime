using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonSlime.Scriptables {

    [CreateAssetMenu(fileName = "LevelDataHolder", menuName = "ScriptableObjects/LevelDataHolderObject", order = 3)]
    public class LevelDataHolder : ScriptableObject {
        public List<LevelData> levelDatas;
        public LevelDataTiles levelDataTiles;

        public LevelData GetLevelDataAt(int index) {
            return levelDatas[index];
        }

        public LevelDataTiles GetLevelDataTiles() {
            return levelDataTiles;
        }
    }
}