using System.Collections.Generic;
using DungeonSlime.Scriptables;
using DungeonSlime.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonSlime.Managers {
    public class LevelManager : MonoBehaviour {

        [Header("tile settings")]
        public Tilemap tilemap;
        
        public LevelDataTiles levelDataTiles;
        public LevelData levelData;
        
        private Level m_currentLevel;
        private bool m_isDead;

        private readonly Dictionary<Block.BlockType, TileBase> m_tiles = new Dictionary<Block.BlockType, TileBase>();

        private void Awake() {
            m_tiles.Add(Block.BlockType.Floor, levelDataTiles.GetFloorTile());
            m_tiles.Add(Block.BlockType.Wall, levelDataTiles.GetWallTile());
            m_tiles.Add(Block.BlockType.Empty, levelDataTiles.GetEmptyTile());
            m_tiles.Add(Block.BlockType.InitialPosition, levelDataTiles.GetInitialPositionTile());
            m_tiles.Add(Block.BlockType.Endgame, levelDataTiles.GetEndPointTile());
            m_tiles.Add(Block.BlockType.Spikes, levelDataTiles.GetSpikesTile());
            m_tiles.Add(Block.BlockType.BlockWall, levelDataTiles.GetBlockWallTile());
            
            LoadLevel();
        }

        private void LoadLevel() {
            m_currentLevel = JsonUtility.FromJson<Level>(levelData.levelJson.text);
            InstantiateLevel(m_currentLevel);
        }

        private void InstantiateLevel(Level level) {
            for (var i = 0; i < level.blocks.Length; i++) {
                var position = GetPositionForIndex(i, level.columnCount);
                tilemap.SetTile(new Vector3Int(position.x, position.y, 0), m_tiles[level.blocks[i].type]);

                //var text = WorldCanvas.Instance.CreateTextAt(tilemap.CellToWorld((Vector3Int)position));
                //text.SetText(string.Format("{0}/{1}", position.x, position.y));
            }
        }

        private static Vector2Int GetPositionForIndex(int index, int columnCount) {
            return new Vector2Int(index % columnCount, index / columnCount);
        }

        public bool IsPlayerDead() {
            return m_isDead;
        }
        
        public bool GetNearestBlock(Vector2Int currentIndex, Vector2Int direction, int depth,
            out Vector2Int nearestIndex, out Block nearestBlock) {

            Vector2Int nextIndex = currentIndex + direction;
            Block block = m_currentLevel.GetBlock(nextIndex);
            
            if (depth == 0) {
                nearestIndex = nextIndex;
                nearestBlock = block;
                return false;
            } 
            
            if (IsWall(block) || IsBlockWall(block)) {
                nearestIndex = nextIndex;
                nearestBlock = block;
                return true;
            }

            if (IsSpike(block)) {
                nearestIndex = nextIndex;
                nearestBlock = block;
                return true;
            }
            
            nearestIndex = nextIndex;
            return GetNearestBlock(nextIndex, direction, depth - 1, out nearestIndex, out nearestBlock);
        }
        
        public bool GetTotalAvailableBlockWithinDepth(Vector2Int currentIndex, Vector2Int direction, int depth,
            int currentAvailableBlocks, bool isCheckingValidPosition, out int totalAvailableBlocks) {
            
            if (depth == 0) {
                totalAvailableBlocks = currentAvailableBlocks;
                return true;
            }

            Vector2Int nextIndex;
            Block block;
            
            if (currentAvailableBlocks == 0 && !isCheckingValidPosition) {
                nextIndex = currentIndex;
                block = m_currentLevel.GetBlock(nextIndex);
            }
            else {
                nextIndex = currentIndex + direction;
                block = m_currentLevel.GetBlock(nextIndex);
            }
            
            
            if (IsWall(block) || IsBlockWall(block)) {
                totalAvailableBlocks = currentAvailableBlocks;
                return false;
            }

            if (IsSpike(block)) {
                m_isDead = true;
            }
            
            currentAvailableBlocks++;
            totalAvailableBlocks = currentAvailableBlocks;
            return GetTotalAvailableBlockWithinDepth(nextIndex, direction, depth - 1, currentAvailableBlocks, isCheckingValidPosition, out totalAvailableBlocks);
        }

        public Vector2Int GetPlayerInitialPosition() {
            for (var i = 0; i < m_currentLevel.height; i++) {
                for (var j = 0; j < m_currentLevel.width; j++) {
                    Block block = m_currentLevel.GetBlock(new Vector2Int(j, i));

                    if (IsPlayerInitialPosition(block)) {
                        return new Vector2Int(j, i);
                    }
                }
            }
            return Vector2Int.zero;
        }
        
        private bool IsWall(Block block) {
            return block.type == Block.BlockType.Wall;
        }

        private bool IsPlayerInitialPosition(Block block) {
            return block.type == Block.BlockType.InitialPosition;
        }

        private bool IsEndGame(Block block) {
            return block.type == Block.BlockType.Endgame;
        }

        private bool IsSpike(Block block) {
            return block.type == Block.BlockType.Spikes;
        }
        
        private bool IsBlockWall(Block block) {
            return block.type == Block.BlockType.BlockWall;
        }
    }
}