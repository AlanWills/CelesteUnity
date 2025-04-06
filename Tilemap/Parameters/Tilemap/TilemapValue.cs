using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps
{
    [CreateAssetMenu(fileName = nameof(TilemapValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Tilemaps/Tilemap Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class TilemapValue : ParameterValue<Tilemap, TilemapValueChangedEvent>
    {
    }
}
