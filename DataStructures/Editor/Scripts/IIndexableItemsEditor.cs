using Celeste.DataStructures;
using Celeste.Objects;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.DataStructures
{
    public class IIndexableItemsEditor<TIndexableItem> : Editor where TIndexableItem : ScriptableObject
    {
        #region Properties and Fields

        protected SerializedProperty ItemsProperty { get; private set; }

        private bool supportsGuids = false;

        #endregion

        private void OnEnable()
        {
            ItemsProperty = serializedObject.FindProperty("items");
            supportsGuids = typeof(IGuid).IsAssignableFrom(typeof(TIndexableItem));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (supportsGuids && GUILayout.Button("Sync Guids"))
            {
                SyncGuids();
            }

            if (GUILayout.Button("Find All"))
            {
                ItemsProperty.FindAssets<TIndexableItem>();
                TrySyncGuids();
            }

            if (GUILayout.Button("Find All In Folder Recursive"))
            {
                ItemsProperty.FindAssets<TIndexableItem>(AssetUtility.GetAssetFolderPath(target));
                TrySyncGuids();
            }

            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                DrawPropertiesExcluding(serializedObject, "m_Script");

                if (changeCheck.changed)
                {
                    TrySyncGuids();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void TrySyncGuids()
        {
            if (supportsGuids)
            {
                SyncGuids();
            }
        }

        protected void SyncGuids()
        {
            if (!supportsGuids)
            {
                Debug.LogAssertion($"Type {typeof(TIndexableItem).Name} does not implement the {nameof(IGuid)} interface.");
                return;
            }

            IIndexableItems<TIndexableItem> indexableItems = target as IIndexableItems<TIndexableItem>;

            for (int i = 0, n = indexableItems.NumItems; i < n; i++)
            {
                TIndexableItem item = indexableItems.GetItem(i);
                IGuid guid = item as IGuid;
                guid.Guid = i + 1;     // 1 index the guids

                EditorUtility.SetDirty(item);
            }

            AssetDatabase.SaveAssets();
        }
    }
}