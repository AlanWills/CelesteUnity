using Celeste.Tilemaps.Catalogues;
using Celeste.Tilemaps.Components;
using CelesteEditor.Components.Catalogue;
using UnityEditor;

namespace CelesteEditor.Tilemaps.Components
{
    [CustomEditor(typeof(TileComponentCatalogue))]
    public class TileComponentCatalogueEditor : ComponentCatalogueEditor<TileComponent>
    {
    }
}
