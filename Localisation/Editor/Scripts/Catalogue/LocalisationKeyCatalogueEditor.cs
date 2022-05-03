using Celeste.Localisation;
using Celeste.Localisation.Catalogue;
using CelesteEditor.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Localisation.Catalogue
{
    [CustomEditor(typeof(LocalisationKeyCatalogue))]
    public class LocalisationKeyCatalogueEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            LocalisationKeyCatalogue localisationKeyCatalogue = target as LocalisationKeyCatalogue;

            EditorGUILayout.LabelField("Num Entries", localisationKeyCatalogue.NumItems.ToString());

            if (GUILayout.Button("Find All"))
            {
                Dictionary<string, LocalisationKey> newLookup = new Dictionary<string, LocalisationKey>();

                foreach (var localisationKey in AssetUtility.FindAssets<LocalisationKey>())
                {
                    newLookup.Add(localisationKey.Key, localisationKey);
                }

                localisationKeyCatalogue.SetItems(newLookup);
                
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }

            base.OnInspectorGUI();
        }
    }
}