using Celeste.UI.Layout;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UI.Layout
{
    [CustomEditor(typeof(SetReferenceLayoutRect))]
    public class SetReferenceLayoutRectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Set"))
            {
                (target as SetReferenceLayoutRect).SetReferenceLayout();
            }

            base.OnInspectorGUI();
        }
    }
}
