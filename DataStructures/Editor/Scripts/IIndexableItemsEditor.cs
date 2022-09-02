using Celeste.DataStructures;
using Celeste.Objects;
using CelesteEditor.Tools;
using System.Collections.Generic;
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

        protected virtual void OnEnable()
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
                AddMissingItemsWithoutReordering(AssetUtility.FindAssets<TIndexableItem>());
            }

            if (GUILayout.Button("Find All In Folder Recursive"))
            {
                AddMissingItemsWithoutReordering(AssetUtility.FindAssets<TIndexableItem>(AssetUtility.GetAssetFolderPath(target)));
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

        private void AddMissingItemsWithoutReordering(List<TIndexableItem> allFoundItems)
        {
            // Add new items without disturbing the order of items we've already added
            // This is especially important with items that have guids
            HashSet<TIndexableItem> currentItems = new HashSet<TIndexableItem>();

            for (int i = 0, n = ItemsProperty.arraySize; i < n; ++i)
            {
                currentItems.Add(ItemsProperty.GetArrayElementAtIndex(i).objectReferenceValue as TIndexableItem);
            }

            List<TIndexableItem> itemsToAdd = new List<TIndexableItem>();

            foreach (TIndexableItem item in allFoundItems)
            {
                if (!currentItems.Contains(item))
                {
                    itemsToAdd.Add(item);
                }
            }

            if (itemsToAdd.Count > 0)
            {
                ItemsProperty.arraySize += itemsToAdd.Count;

                int offset = currentItems.Count;
                for (int i = 0, n = itemsToAdd.Count; i < n; ++i)
                {
                    ItemsProperty.GetArrayElementAtIndex(offset + i).objectReferenceValue = itemsToAdd[i];
                }

                serializedObject.ApplyModifiedProperties();

                OnNewItemsAdded();
            }
            
            TrySyncGuids();
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

        protected virtual void OnNewItemsAdded()
        {
        }
    }
}