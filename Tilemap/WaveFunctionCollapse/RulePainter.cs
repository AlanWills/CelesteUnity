using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
