using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps
{
    [CreateAssetMenu(fileName = "TilemapValue", menuName = "Celeste/Parameters/Tilemaps/Tilemap Value")]
    public class TilemapValue : ParameterValue<Tilemap>
    {
    }
}
