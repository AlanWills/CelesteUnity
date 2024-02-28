using Celeste.Components.Catalogue;
using Celeste.Tilemaps.Components;
using UnityEngine;

namespace Celeste.Tilemaps.Catalogues
{
    [CreateAssetMenu(fileName = nameof(TileComponentCatalogue), order = CelesteMenuItemConstants.TILEMAPS_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TILEMAPS_MENU_ITEM + "Components/Tile Component Catalogue")]
    public class TileComponentCatalogue : ComponentCatalogue<TileComponent>
    {
    }
}
