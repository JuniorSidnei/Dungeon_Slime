using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DungeonSlime.Scriptables;
using DungeonSlime.Utils;
using UnityEngine;

namespace DungeonSlime.Managers {

    public class LevelGenerator : MonoBehaviour {

        //public Texture2D LevelInfo;
        //public int levelId;

        //public List<TilesData> PixelMappings;

        public LevelData levelData;
        public LevelDataTiles LevelDataTiles;
        private List<int> m_pixelMappingsId = new List<int>();
        private Level m_level = new Level();
        
        private void Start() {
            GenerateJsonFromTexture();
        }

        [ContextMenu("Generate Json From Texture")]
        private void GenerateJsonFromTexture() {
            var width = levelData.LevelTexture.width;
            var height = levelData.LevelTexture.height;
            
            m_level.height = height;
            m_level.width = width;
            
            m_level.blocks = new Block[width * height];
            var index = 0;
            for (var i = 0; i < height; i++) {
                for (var j = 0; j < width; j++) {
                    GenerateTile(j, i, index);
                    index++;
                }
            }
            
            var json = JsonUtility.ToJson(m_level);
            using (var fs = new FileStream(string.Format("Assets/LevelJsons/level_{0}.json", levelData.CurrentLevelData), FileMode.Create)) {
                using (var writer = new StreamWriter(fs)) {
                    writer.Write(json);
                }
            }
        }

        private void GenerateTile(int x, int y, int index) {
            var pixelColor = levelData.LevelTexture.GetPixel(x, y);
            
            foreach (var pixel in LevelDataTiles.tileDatas) {
                if (pixel.TileColor.Equals(pixelColor)) {
                    m_pixelMappingsId.Add(pixel.TileId);
                    m_level.blocks[index] = new Block {type = (Block.BlockType) pixel.TileId};
                }
            }
        }
    }
}