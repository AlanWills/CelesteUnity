using Celeste.DataImporters;
using CelesteEditor.DataStructures;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.DataImporters
{
    [CustomEditor(typeof(DataImporterCatalogue))]
    public class DataImporterCatalogueEditor : IIndexableItemsEditor<DataImporter>
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Import All"))
            {
                DataImporterCatalogue dataImporterCatalogue = target as DataImporterCatalogue;
                EditorCoroutineUtility.StartCoroutine(dataImporterCatalogue.ImportAll(), target);
            }

            base.OnInspectorGUI();
        }
    }
}
