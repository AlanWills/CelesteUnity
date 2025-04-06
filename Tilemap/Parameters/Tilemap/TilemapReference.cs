using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps
{
    [CreateAssetMenu(fileName = nameof(TilemapReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Tilemaps/Tilemap Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class TilemapReference : ParameterReference<Tilemap, TilemapValue, TilemapReference>
    {
    }
}