using Celeste.DataImporters;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.DataImporters
{
    [CustomEditor(typeof(DataImporter), true)]
    public class DataImporterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Import"))
            {
                DataImporter importer = target as DataImporter;
                EditorCoroutineUtility.StartCoroutine(importer.Import(), importer);
            }

            base.OnInspectorGUI();
        }
    }
}
