using Celeste.Localisation;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Localisation.Catalogue
{
    [CustomEditor(typeof(LocalisationKeyCategoryCatalogue))]
    public class LocalisationKeyCategoryCatalogueEditor : IIndexableItemsEditor<LocalisationKeyCategory>
    {
    }
}