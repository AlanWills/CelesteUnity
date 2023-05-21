using UnityEngine.Tilemaps;
using UnityEngine;
using Celeste.Components;

namespace Celeste.Tilemaps
{
    public interface ITileDataComponent
    {
        void GetTileData(Instance instance, Vector3Int position, ITilemap tilemap, ref TileData tileData);
    }
}
