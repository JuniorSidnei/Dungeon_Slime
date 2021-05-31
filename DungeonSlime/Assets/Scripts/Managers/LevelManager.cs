using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Utils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonSlime {
    public class LevelManager : MonoBehaviour {

        //public Tile tile;
        public GameObject foo; // todo this will be a tile
        public TextAsset jsonFile;
        private Level m_currentLevel;
        
        public void LoadLevel(int levelIndex) {
            m_currentLevel = JsonUtility.FromJson<Level>(jsonFile.text);
            InstantiateLevel(m_currentLevel);
        }

        private void InstantiateLevel(Level level) {
            for (var i = 0; i < level.blocks.Length; i++) {
                var position = GetPositionForIndex(i, level.columnCount);
                Instantiate(foo, new Vector3(position.x, position.y, 0), quaternion.identity, transform);
            }
        }

        private static Vector2Int GetPositionForIndex(int index, int columnCount) {
            return new Vector2Int(index % columnCount, index / columnCount);
        }
    }
}