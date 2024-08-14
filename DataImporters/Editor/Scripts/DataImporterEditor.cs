using Celeste.DataImporters;
#if USE_EDITOR_COROUTINES
using Unity.EditorCoroutines.Editor;
#endif
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.DataImporters
{
    [CustomEditor(typeof(DataImporter), true)]
    public class DataImporterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
#if USE_EDITOR_COROUTINES
            if (GUILayout.Button("Import"))
            {
                DataImporter importer = target as DataImporter;
                EditorCoroutineUtility.StartCoroutine(importer.Import(), importer);
            }
#endif
            base.OnInspectorGUI();
        }
    }
}
