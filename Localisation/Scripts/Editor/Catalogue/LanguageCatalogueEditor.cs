using Celeste.Localisation;
using Celeste.Localisation.Catalogue;
using CelesteEditor.DataStructures;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Localisation.Catalogue
{
    [CustomEditor(typeof(LanguageCatalogue))]
    public class LanguageCatalogueEditor : IIndexableItemsEditor<Language>
    {
    }
}