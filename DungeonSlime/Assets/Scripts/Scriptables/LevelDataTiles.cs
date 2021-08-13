using System.Collections;
using System.Collections.Generic;
using DungeonSlime.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonSlime.Scriptables {

    [CreateAssetMenu(fileName = "LevelDataTiles", menuName = "ScriptableObjects/TileDataObject", order = 1)]
    public class LevelDataTiles : ScriptableObject {
        
        [Header("Tiles infos from level")]
        public List<TilesData> tileDatas;

        public TileBase GetFloorTile() {
            return tileDatas[0].Tile;
        }
        
        public TileBase GetWallTile() {
            return tileDatas[1].Tile;
        }
        
        public TileBase GetEndPointTile() {
            return tileDatas[2].Tile;
        }
        
        public TileBase GetInitialPositionTile() {
            return tileDatas[3].Tile;
        }
        
        public TileBase GetEmptyTile() {
            return tileDatas[4].Tile;
        }
        
    }
}