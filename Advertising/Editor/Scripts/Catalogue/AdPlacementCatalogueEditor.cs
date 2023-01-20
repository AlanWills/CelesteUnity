using Celeste.Advertising;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Advertising
{
    [CustomEditor(typeof(AdPlacementCatalogue))]
    public class AdPlacementCatalogueEditor : IIndexableItemsEditor<AdPlacement>
    {
    }
}
