using Celeste.Narrative.Characters;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Narrative.Backgrounds
{
    [CustomEditor(typeof(BackgroundCatalogue))]
    public class BackgroundCatalogueEditor : IIndexableItemsEditor<Background>
    {
    }
}