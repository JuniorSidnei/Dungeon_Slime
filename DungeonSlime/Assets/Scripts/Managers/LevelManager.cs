using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Utils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonSlime {
    public class LevelManager : MonoBehaviour {

        [Header("tile settings")]
        public TileBase wall;
        public TileBase floor;
        public TileBase empty;
        public Tilemap tileMap;
        
        [Header("json file")]
        public TextAsset jsonFile;
        private Level m_currentLevel;
        
        public void LoadLevel(int levelIndex) {
            m_currentLevel = JsonUtility.FromJson<Level>(jsonFile.text);
            InstantiateLevel(m_currentLevel);
        }

        private void InstantiateLevel(Level level) {
            for (var i = 0; i < level.blocks.Length; i++) {
                var position = GetPositionForIndex(i, level.columnCount);
                
                if (level.blocks[i].type == Block.BlockType.Floor) {
                    tileMap.SetTile(new Vector3Int(position.x, position.y, 0), floor);    
                }
                else if(level.blocks[i].type == Block.BlockType.Wall) {
                    tileMap.SetTile(new Vector3Int(position.x, position.y, 0), wall);    
                }
                else {
                    tileMap.SetTile(new Vector3Int(position.x, position.y, 0), empty);
                }
            }
        }

        private static Vector2Int GetPositionForIndex(int index, int columnCount) {
            return new Vector2Int(index % columnCount, index / columnCount);
        }
    }
}