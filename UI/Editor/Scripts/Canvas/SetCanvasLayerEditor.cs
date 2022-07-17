using Celeste.UI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UI
{
    [CustomEditor(typeof(SetCanvasLayer))]
    public class SetCanvasLayerEditor : Editor
    {
        public SetCanvasLayer SetCanvasLayer => target as SetCanvasLayer;

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Apply"))
            {
                SetCanvasLayer.Apply();
            }

            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();

                if (changeCheck.changed)
                {
                    SetCanvasLayer.Apply();
                }
            }
        }
    }
}