using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps
{
    public static class TilemapUtility
    {
        public static void ClearAllTilesNoResize(this Tilemap tilemap)
        {
            int height = tilemap.Height();
            int width = tilemap.Width();

            tilemap.ClearAllTiles();
            tilemap.size = new Vector3Int(width, height, 1);
            tilemap.ResizeBounds();
        }

        public static int Area(this Tilemap tilemap)
        {
            return Area(tilemap.cellBounds);
        }

        public static int Width(this Tilemap tilemap)
        {
            return tilemap.cellBounds.size.x;
        }

        public static int Height(this Tilemap tilemap)
        {
            return tilemap.cellBounds.size.y;
        }

        public static int Area(this BoundsInt tilemapBounds)
        {
            return tilemapBounds.size.x * tilemapBounds.size.y;
        }

        public static int Width(this BoundsInt tilemapBounds)
        {
            return tilemapBounds.size.x;
        }

        public static int Height(this BoundsInt tilemapBounds)
        {
            return tilemapBounds.size.y;
        }
    }
}
