using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.DataStructures
{
    public class IIndexableItemsEditor<TIndexableItem> : Editor where TIndexableItem : ScriptableObject
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
                ItemsProperty.FindAssets<TIndexableItem>();
            }

            if (GUILayout.Button("Find All In Folder Recursive"))
            {
                ItemsProperty.FindAssets<TIndexableItem>(AssetUtility.GetAssetFolderPath(target));
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}