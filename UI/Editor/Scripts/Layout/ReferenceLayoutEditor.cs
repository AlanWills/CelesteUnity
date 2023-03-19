using Celeste.UI.Layout;
using UnityEditor;

namespace CelesteEditor.UI.Layout
{
    [CustomEditor(typeof(ReferenceLayout))]
    public class ReferenceLayoutEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            ReferenceLayout referenceLayout = target as ReferenceLayout;
            string referenceLayoutName =
                referenceLayout.rectTransform != null ? referenceLayout.rectTransform.name : "Not Yet Set...";
            EditorGUILayout.LabelField(referenceLayoutName);
        }
    }
}