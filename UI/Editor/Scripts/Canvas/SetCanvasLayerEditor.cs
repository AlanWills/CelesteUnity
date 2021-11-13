using Celeste.UI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UI
{
    [CustomEditor(typeof(SetCanvasLayer))]
    public class SetCanvasLayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Apply"))
            {
                (target as SetCanvasLayer).Apply();
                EditorUtility.SetDirty(target);
            }

            base.OnInspectorGUI();
        }
    }
}
