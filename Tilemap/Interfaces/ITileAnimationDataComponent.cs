using UnityEngine.Tilemaps;
using UnityEngine;
using Celeste.Components;

namespace Celeste.Tilemaps
{
    public interface ITileAnimationDataComponent
    {
        bool GetTileAnimationData(
            Instance instance, 
            Vector3Int position, 
            ITilemap tilemap, 
            ref TileAnimationData tileAnimationData);
    }
}
