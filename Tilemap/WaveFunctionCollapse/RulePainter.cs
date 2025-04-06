using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps.WaveFunctionCollapse
{
    [AddComponentMenu("Celeste/Tilemaps/Wave Function Collapse/Rule Painter")]
    public class RulePainter : MonoBehaviour
    {
        public Tilemap tilemap;
        public TileDescription tileDescription;
        public TilemapSolver tilemapSolver;
    }
}
