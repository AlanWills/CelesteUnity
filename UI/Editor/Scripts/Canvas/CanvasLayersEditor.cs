using Celeste.UI;
using CelesteEditor.DataStructures;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UI
{
    [CustomEditor(typeof(CanvasLayers))]
    public class CanvasLayersEditor : IIndexableItemsEditor<CanvasLayer>
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            if (GUILayout.Button("Synchronize"))
            {
                (target as CanvasLayers).Synchronize();
            }

            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                (target as CanvasLayers).Synchronize();
            }
        }
    }
}
