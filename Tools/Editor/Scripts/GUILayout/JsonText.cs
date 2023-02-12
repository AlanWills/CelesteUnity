using Newtonsoft.Json.Linq;
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
                        JToken jt = JToken.Parse(property.stringValue);
                        property.stringValue = jt.ToString(Newtonsoft.Json.Formatting.Indented);
                    }

                    property.stringValue = EditorGUILayout.TextArea(property.stringValue);
                }
            }
        }
    }
}
