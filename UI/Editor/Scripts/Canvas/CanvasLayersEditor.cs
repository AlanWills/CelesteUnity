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
            CanvasLayers canvasLayers = target as CanvasLayers;

            if (GUILayout.Button("Synchronize"))
            {
                canvasLayers.Synchronize();
            }

            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();

                if (changeCheck.changed)
                {
                    canvasLayers.Synchronize();
                }
            }
        }
    }
}
