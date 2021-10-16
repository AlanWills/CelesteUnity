using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.DataStructures
{
    public class IIndexableItemsEditor<T> : Editor where T : ScriptableObject
    {
        #region Properties and Fields

        protected SerializedProperty ItemsProperty { get; private set; }

        #endregion

        private void OnEnable()
        {
            ItemsProperty = serializedObject.FindProperty("items");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Find All"))
            {
                ItemsProperty.FindAssets<T>();
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}