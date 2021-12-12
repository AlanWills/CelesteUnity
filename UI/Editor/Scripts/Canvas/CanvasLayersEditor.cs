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

            CanvasLayers canvasLayers = target as CanvasLayers;

            if (GUILayout.Button("Synchronize"))
            {
                canvasLayers.Synchronize();
            }

            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                canvasLayers.Synchronize();
            }
        }
    }
}
