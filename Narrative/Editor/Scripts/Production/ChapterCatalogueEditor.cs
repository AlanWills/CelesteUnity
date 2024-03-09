using Celeste.Narrative;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Narrative
{
    [CustomEditor(typeof(ChapterCatalogue))]
    public class ChapterCatalogueEditor : IIndexableItemsEditor<Chapter>
    {
    }
}