using Celeste.Scene.Hierarchy;
using CelesteEditor.Objects;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Scene.Hierarchy
{
    [CustomEditor(typeof(StringGameObjectDictionary))]
    public class StringGameObjectDictionaryEditor : DictionaryScriptableObjectEditor<string, GameObject>
    {
        private string key;
        private GameObject value;

        public override void OnInspectorGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                key = EditorGUILayout.TextField(key);

                using (var changeCheck = new EditorGUI.ChangeCheckScope())
                {
                    value = EditorGUILayout.ObjectField(value, typeof(GameObject), false) as GameObject;

                    if (changeCheck.changed && string.IsNullOrEmpty(key))
                    {
                        key = GetKey(value);
                    }
                }

                if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
                {
                    (target as StringGameObjectDictionary).AddItem(key, value);
                    key = "";
                    value = null;

                    RefreshData();
                }
            }
            
            base.OnInspectorGUI();
        }

        protected override string GetKey(GameObject item)
        {
            return item.name;
        }
    }
}
