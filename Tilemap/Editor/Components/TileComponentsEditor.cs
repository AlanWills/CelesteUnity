using Celeste.Tilemaps.Components;
using Celeste.Tilemaps.Tiles;
using CelesteEditor.Components;
using UnityEditor;

namespace CelesteEditor.Tilemaps.Components
{
    [CustomEditor(typeof(TileComponents))]
    public class TileComponentsEditor : ComponentContainerUsingTemplatesEditor<TileComponent>
    {
    }
}
