using Celeste.DataImporters;
using CelesteEditor.DataStructures;
#if USE_EDITOR_COROUTINES
using Unity.EditorCoroutines.Editor;
using UnityEngine;
#endif
using UnityEditor;

namespace CelesteEditor.DataImporters
{
    [CustomEditor(typeof(DataImporterCatalogue))]
    public class DataImporterCatalogueEditor : IIndexableItemsEditor<DataImporter>
    {
        public override void OnInspectorGUI()
        {
#if USE_EDITOR_COROUTINES
            if (GUILayout.Button("Import All"))
            {
                DataImporterCatalogue dataImporterCatalogue = target as DataImporterCatalogue;
                EditorCoroutineUtility.StartCoroutine(dataImporterCatalogue.ImportAll(), target);
            }
#endif
            base.OnInspectorGUI();
        }
    }
}