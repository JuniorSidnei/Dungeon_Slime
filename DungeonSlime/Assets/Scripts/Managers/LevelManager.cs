using System;
using System.Collections.Generic;
using DG.Tweening;
using DungeonSlime.Character;
using DungeonSlime.Scriptables;
using DungeonSlime.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonSlime.Managers {
    public class LevelManager : MonoBehaviour {

        [Header("tile settings")]
        public Tilemap tilemap;

        
        [Header("movement limit settings")]
        public GameObject movementLimitBox;
        public TextMeshProUGUI movementLimitText;
        
        [Header("level settings")]
        public GameObject levelBox;
        public TextMeshProUGUI levelText;
        public float transitionTime;
        public float textBoxOffset;
        
        [Header("level data settings")]
        public LevelDataTiles levelDataTiles;
        public LevelData levelData;
        public bool showTilesIndex;
        
        private Level m_currentLevel;
        private bool m_isLevelClear;
        private UserData m_userData;
        
        public bool IsObjectDead { get; set; }

        public bool IsLevelClear => m_isLevelClear;

        public UserData UserData => m_userData;

        private readonly Dictionary<Block.BlockType, TileBase> m_tiles = new Dictionary<Block.BlockType, TileBase>();

        private void Awake() {
            GameManager.Instance.GlobalDispatcher.Subscribe<OnFinishMovement>(OnFinishMovement);
            
            m_tiles.Add(Block.BlockType.Floor, levelDataTiles.GetFloorTile());
            m_tiles.Add(Block.BlockType.Wall, levelDataTiles.GetWallTile());
            m_tiles.Add(Block.BlockType.Empty, levelDataTiles.GetEmptyTile());
            m_tiles.Add(Block.BlockType.InitialPosition, levelDataTiles.GetInitialPositionTile());
            m_tiles.Add(Block.BlockType.Endgame, levelDataTiles.GetEndPointTile());
            m_tiles.Add(Block.BlockType.Spikes, levelDataTiles.GetSpikesTile());
            m_tiles.Add(Block.BlockType.BlockWall, levelDataTiles.GetBlockWallTile());
            m_tiles.Add(Block.BlockType.FakeSpikes, levelDataTiles.GetFakeSpikesTile());

            m_userData = SaveManager.LoadData();
            movementLimitBox.SetActive(m_userData.levelDifficulty == 1);
            movementLimitText.gameObject.SetActive(m_userData.levelDifficulty == 1);
            UpdateMovementLimitValue(levelData.numberOfMovements);
            LoadLevel();
        }

        private void Start() {
            levelText.text = string.Format("LEVEL {0}", levelData.currentLevelData);
            var originalPos = levelBox.transform.position;
            levelBox.transform.DOMoveX(textBoxOffset, transitionTime).OnComplete(() => {
                levelBox.transform.DOMoveX(originalPos.x, 2f);
            });
        }

        private void OnFinishMovement(OnFinishMovement ev) {
            if (!m_isLevelClear || ev.CharacterType != CharacterStates.CharacterType.Slime) return;
            
            GameManager.Instance.LoadNextScene();
        }
        
        private void LoadLevel() {
            m_currentLevel = JsonUtility.FromJson<Level>(levelData.levelJson.text);
            InstantiateLevel(m_currentLevel);
        }

        private void InstantiateLevel(Level level) {
            for (var i = 0; i < level.blocks.Length; i++) {
                var position = GetPositionForIndex(i, level.columnCount);
                tilemap.SetTile(new Vector3Int(position.x, position.y, 0), m_tiles[level.blocks[i].type]);

                if (!showTilesIndex) continue;
                
                var text = WorldCanvas.Instance.CreateTextAt(tilemap.CellToWorld((Vector3Int) position));
                text.SetText(string.Format("{0}/{1}", position.x, position.y));
            }
        }

        private static Vector2Int GetPositionForIndex(int index, int columnCount) {
            return new Vector2Int(index % columnCount, index / columnCount);
        }

        public void UpdateMovementLimitValue(int value) {
            movementLimitText.text = value.ToString();
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
                IsObjectDead = true;
            }

            if (IsEndGame(block)) {
                m_isLevelClear = true;
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