using Celeste.Localisation;
using Celeste.Localisation.Catalogue;
using CelesteEditor.DataStructures;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Localisation.Catalogue
{
    [CustomEditor(typeof(LanguageCatalogue))]
    public class LanguageCatalogueEditor : IIndexableItemsEditor<Language>
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Find Unused Keys In Project"))
            {
                LanguageCatalogue languageCatalogue = target as LanguageCatalogue;

                foreach (LocalisationKey localisationKey in EditorOnly.FindAssets<LocalisationKey>())
                {
                    bool used = false;

                    for (int i = 0, n = languageCatalogue.NumItems; i < n; ++i)
                    {
                        Language language = languageCatalogue.GetItem(i);
                        
                        if (language.HasKey(localisationKey))
                        {
                            used = true;
                            break;
                        }
                    }

                    if (!used)
                    {
                        Debug.LogAssertion($"Localisation Key '{localisationKey.name}' is not used in any language and can probably be deleted.");
                    }
                }
            }

            base.OnInspectorGUI();
        }
    }
}