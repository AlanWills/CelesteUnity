using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps
{
    [CreateAssetMenu(fileName = nameof(TilemapValue), menuName = "Celeste/Parameters/Tilemaps/Tilemap Value")]
    public class TilemapValue : ParameterValue<Tilemap>
    {
    }
}
