using Celeste.Features;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Features
{
    [CustomEditor(typeof(FeatureCatalogue))]
    public class FeatureCatalogueEditor : IIndexableItemsEditor<Feature>
    {
    }
}
