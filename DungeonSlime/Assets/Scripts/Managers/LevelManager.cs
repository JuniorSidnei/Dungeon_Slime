using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DungeonSlime.Utils;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonSlime.Managers {
    public class LevelManager : MonoBehaviour {

        [Header("tile settings")]
        public TileBase wallTile;
        public TileBase floorTile;
        public TileBase empty;
        public Tilemap wallMap;
        public Tilemap floorMap;
        public TextMeshProUGUI textInfo;
        
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
                    floorMap.SetTile(new Vector3Int(position.x, position.y, 0), floorTile);    
                }
                else if(level.blocks[i].type == Block.BlockType.Wall) {
                    wallMap.SetTile(new Vector3Int(position.x, position.y, 0), wallTile);
                }
                else {
                    floorMap.SetTile(new Vector3Int(position.x, position.y, 0), empty);
                }
                //textInfo.SetText(position.x + " / " + position.y);
                //Instantiate(textInfo, new Vector3(position.x, position.y, 0), quaternion.identity, transform);
            }
        }

        private static Vector2Int GetPositionForIndex(int index, int columnCount) {
            return new Vector2Int(index % columnCount, index / columnCount);
        }
        
        public bool GetFarthestBlock(Vector2Int currentIndex, Vector2Int direction, out Vector2Int farthestIndex, out Block farthestBlock) {
            Vector2Int nextIndex = currentIndex + direction;
           
            Block block = m_currentLevel.getBlock(nextIndex);
            
            if (IsWall(block)) {
                farthestIndex = nextIndex;
                farthestBlock = block;
                return true;
            }
            
            farthestIndex = nextIndex;
            farthestBlock = block;
            return GetFarthestBlock(nextIndex, direction, out farthestIndex, out block);
        }

        private bool IsWall(Block block) {
            return block.type == Block.BlockType.Wall;
        }
    }
}