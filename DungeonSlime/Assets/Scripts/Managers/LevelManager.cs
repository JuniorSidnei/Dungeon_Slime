using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DungeonSlime.Utils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonSlime {
    public class LevelManager : MonoBehaviour {

        [Header("tile settings")]
        public TileBase wallTile;
        public TileBase floorTile;
        public TileBase empty;
        public Tilemap wallMap;
        public Tilemap floorMap;
        
        [Header("json file")]
        public TextAsset jsonFile;

        [Header("player")]
        public GameObject Player;
        
        private Level m_currentLevel;
        private List<Vector3> availablePlaces = new List<Vector3>();
        
        public void LoadLevel(int levelIndex) {
            //TODO:: load level by index
            m_currentLevel = JsonUtility.FromJson<Level>(jsonFile.text);
            InstantiateLevel(m_currentLevel);
            
            for (var n = wallMap.cellBounds.xMin; n < wallMap.cellBounds.xMax; n++)  {
                for (var p = wallMap.cellBounds.yMin; p < wallMap.cellBounds.yMax; p++)  {
                    var localPlace = (new Vector3Int(n, p, (int)wallMap.transform.position.y));
                    var place = wallMap.CellToWorld(localPlace);
                    
                    if (wallMap.HasTile(localPlace))  {
                        //Tile at "place"
                        availablePlaces.Add(place);
                    }
                }
            }
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
            }
        }

        private static Vector2Int GetPositionForIndex(int index, int columnCount) {
            return new Vector2Int(index % columnCount, index / columnCount);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                Vector2Int currentPlayerPosition = (Vector2Int) wallMap.WorldToCell(Player.transform.position);
                
                if (Move(currentPlayerPosition, Vector2Int.right, out Vector2Int toPosition)) {
                    Player.transform.DOMoveX(toPosition.x, 1f);
                }
            }
        }

        private bool Move(Vector2Int currentIndex, Vector2Int direction, out Vector2Int farthestIndex) {
            var nextIndex = currentIndex + direction;
            Block block = m_currentLevel.getBlock(nextIndex);
            
            if (IsWall(block)) {
                farthestIndex = nextIndex;
                return true;
            }
            
            farthestIndex = nextIndex;
            return Move(nextIndex, direction, out farthestIndex);
        }

        private bool IsWall(Block block) {
            return block.type == Block.BlockType.Wall;
        }
    }
}