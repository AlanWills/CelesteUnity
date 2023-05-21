using Celeste.Components.Catalogue;
using Celeste.Tilemaps.Components;
using UnityEngine;

namespace Celeste.Tilemaps.Catalogues
{
    [CreateAssetMenu(fileName = nameof(TileComponentCatalogue), menuName = "Celeste/Tiles/Components/Tile Component Catalogue")]
    public class TileComponentCatalogue : ComponentCatalogue<TileComponent>
    {
    }
}
