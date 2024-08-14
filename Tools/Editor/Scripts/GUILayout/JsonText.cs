using UnityEditor;
using UnityEngine;

namespace CelesteEditor
{
    public partial class CelesteEditorGUILayout
    {
        public static void JsonText(string label, SerializedProperty property)
        {
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label);

            if (property.isExpanded)
            {
                using (var horizontal = new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Prettify", GUILayout.ExpandWidth(false)))
                    {
                        object jsonObject = JsonUtility.FromJson<object>(property.stringValue);
                        property.stringValue = JsonUtility.ToJson(jsonObject, true);
                    }

                    property.stringValue = EditorGUILayout.TextArea(property.stringValue);
                }
            }
        }
    }
}
