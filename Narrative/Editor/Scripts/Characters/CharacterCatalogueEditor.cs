using Celeste.Narrative.Characters;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Narrative.Characters
{
    [CustomEditor(typeof(CharacterCatalogue))]
    public class CharacterCatalogueEditor : IIndexableItemsEditor<Character>
    {
    }
}