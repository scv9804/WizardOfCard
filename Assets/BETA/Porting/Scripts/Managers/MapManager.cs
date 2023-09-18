using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BETA.Singleton;

using Sirenix.OdinInspector;

using System;

using UnityEngine.Tilemaps;

namespace BETA.Porting
{
    // ==================================================================================================== MapManager

    public sealed class MapManager : SingletonMonoBehaviour<MapManager>
    {
        // ==================================================================================================== Field

        // =========================================================================== Map

        private Entity _entity;

        // =========================================================================== Map

        private Dictionary<Vector2Int, OverlayTile> _map;

        private MapBounds _bounds;

        [FoldoutGroup("타일맵")]
        public Tilemap Tilemap;

        // =========================================================================== Tile

        [FoldoutGroup("타일 데이터")]
        public Dictionary<TileBase, TileScriptableData> TemporaryTileData = new Dictionary<TileBase, TileScriptableData>();

        // =========================================================================== Overlay

        [ShowInInspector] [FoldoutGroup("오버레이 타일")]
        private Dictionary<Color, List<OverlayTile>> _overlayTiles = new Dictionary<Color, List<OverlayTile>>();

        // ==================================================================================================== Method

        // =========================================================================== Singleton

        protected override bool Initialize()
        {
            var isEmpty = base.Initialize();

            if (isEmpty)
            {
                name = "Map Manager";

                DontDestroyOnLoad(gameObject);
            }

            return isEmpty;
        }

        //

        public void SetTemporaryTileData()
        {
            //foreach (var data in DataManager.Instance.Tiles.Data)
            //{
            //    foreach (var item in data.TileBase)
            //    {
            //        TemporaryTileData.Add(item, data);
            //    }
            //}
        }

        //public void SetMap()
        //{
        //    Tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();

        //    var bounds = Tilemap.cellBounds;

        //    _bounds = new MapBounds(bounds.xMax, bounds.yMax, bounds.xMin, bounds.yMin);

        //    var prefab = DataManager.Instance.OverlayTilePrefab;

        //    for (int z = bounds.max.z; z >= bounds.min.z; z--)
        //    {
        //        for (int y = bounds.min.y; y < bounds.max.y; y++)
        //        {
        //            for (int x = bounds.min.x; x < bounds.max.x; x++)
        //            {
        //                var tileLocation = new Vector3Int(x, y, z);
        //                var tileKey = new Vector2Int(x, y);

        //                if (Tilemap.HasTile(tileLocation) && !_map.ContainsKey(tileKey))
        //                {
        //                    var overlayTile = Instantiate(prefab, transform);
        //                    var cellWorldPosition = Tilemap.GetCellCenterWorld(tileLocation);
        //                    var baseTile = Tilemap.GetTile(tileLocation);

        //                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
        //                    overlayTile.GetComponent<SpriteRenderer>().sortingOrder = Tilemap.GetComponent<TilemapRenderer>().sortingOrder;
        //                    overlayTile.Location3D = tileLocation;

        //                    if (TemporaryTileData.ContainsKey(baseTile))
        //                    {
        //                        overlayTile.tileData = TemporaryTileData[baseTile];
        //                        if (TemporaryTileData[baseTile].type == Enums.TileType.NON_TRAVERSABLE)
        //                            overlayTile.isBlocked = true;
        //                    }

        //                    _map.Add(tileKey, overlayTile);
        //                }
        //            }
        //        }
        //    }
        //}
    }

    // ==================================================================================================== MapBounds

    [Serializable]
    public class MapBounds
    {
        // ==================================================================================================== Field

        // =========================================================================== Map

        public int xMax = 0;
        public int xMin = 0;

        public int yMax = 0;
        public int yMin = 0;

        // ==================================================================================================== Method

        // =========================================================================== Constructor

        public MapBounds() { }

        public MapBounds(int xMax, int xMin, int yMax, int yMin)
        {
            this.xMax = xMax;
            this.xMin = xMin; 

            this.yMax = yMax;
            this.yMin = yMin;
        }
    }
}
