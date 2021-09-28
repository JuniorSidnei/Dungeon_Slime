using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DungeonSlime.Scriptables;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Managers {

    public class LevelGenerator : MonoBehaviour {

        public LevelDataHolder levelDataHolder;
        private List<int> m_pixelMappingsId;
        private Level m_level;
        public int currentLevelIndex;
        
        [ContextMenu("Generate Json From Texture")]
        public void GenerateJsonFromTexture() {
            var currentData = levelDataHolder.GetLevelDataAt(currentLevelIndex);
            var levelDataTiles = levelDataHolder.GetLevelDataTiles();
            
            var width = currentData.levelTexture.width;
            var height = currentData.levelTexture.height;
            
            m_pixelMappingsId = new List<int>();
            
            m_level = new Level {height = height, width = width, blocks = new Block[width * height]};

            var index = 0;
            for (var i = 0; i < height; i++) {
                for (var j = 0; j < width; j++) {
                    GenerateTile(currentData, levelDataTiles, j, i, index);
                    index++;
                }
            }
            
            var json = JsonUtility.ToJson(m_level);
            using (var fs = new FileStream(string.Format("Assets/LevelJsons/level_{0}.json", ++currentLevelIndex), FileMode.Create)) {
                using (var writer = new StreamWriter(fs)) {
                    writer.Write(json);
                }
            }
        }

        private void GenerateTile(LevelData currentData, LevelDataTiles levelDataTiles, int x, int y, int index) {
            var pixelColor = currentData.levelTexture.GetPixel(x, y);
            
            foreach (var pixel in levelDataTiles.tileDatas) {
                if (pixel.TileColor.Equals(pixelColor)) {
                    m_pixelMappingsId.Add(pixel.TileId);
                    m_level.blocks[index] = new Block {type = (Block.BlockType) pixel.TileId};
                }
            }
        }
    }
}