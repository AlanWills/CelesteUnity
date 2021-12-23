using Celeste.Narrative.Characters;
using CelesteEditor.DataStructures;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.Characters
{
    [CustomEditor(typeof(CharacterCustomisationCatalogue))]
    public class CharacterCustomisationCatalogueEditor : IIndexableItemsEditor<CharacterCustomisation>
    {
    }
}