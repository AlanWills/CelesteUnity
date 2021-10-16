using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps.WaveFunctionCollapse
{
    [Serializable]
    public class Rule : ScriptableObject
    {
        public Direction direction;
        public TileDescription otherTile;
    }
}

// IMPROVEMENT: Have a rule which allows empty tiles?