using Celeste.Objects;
using Celeste.Tools;
using CelesteEditor.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Objects
{
    public abstract class DictionaryScriptableObjectEditor<TKey, TValue> : Editor where TValue : Object
    {
        #region Properties and Fields

        private int currentPage = 0;
        private List<(TKey, TValue)> data = new List<(TKey, TValue)>();

        private const int ENTRIES_PER_PAGE = 40;

        #endregion

        protected virtual void OnEnable()
        {
            RefreshData();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Find All"))
            {
                FindAssets(string.Empty);
            }

            if (GUILayout.Button("Find All In Folder Recursive"))
            {
                FindAssets(AssetUtility.GetAssetFolderPath(target));
            }

            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                DrawPropertiesExcluding(serializedObject, "m_Script");

                currentPage = GUIExtensions.PaginatedList(
                    currentPage,
                    ENTRIES_PER_PAGE,
                    data.Count,
                    (i) =>
                    {
                        DrawEntry(data[i].Item1, data[i].Item2);
                    },
                    () => false,
                    () => GUILayout.Button("-", GUILayout.ExpandWidth(false)),
                    () => { },
                    (i) =>
                    {
                        TKey key = data[i].Item1;
                        
                        DictionaryScriptableObject<TKey, TValue> dictionary = target as DictionaryScriptableObject<TKey, TValue>;
                        dictionary.RemoveItem(key);

                        data.RemoveAt(i);
                    });
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void FindAssets(string directory)
        {
            DictionaryScriptableObject<TKey, TValue> dictionary = target as DictionaryScriptableObject<TKey, TValue>;
            dictionary.Clear();

            foreach (TValue value in AssetUtility.FindAssets<TValue>(string.Empty, directory))
            {
                TKey key = GetKey(value);
                dictionary.AddItem(key, value);
            }

            RefreshData();
        }

        protected void RefreshData()
        {
            DictionaryScriptableObject<TKey, TValue> dictionary = target as DictionaryScriptableObject<TKey, TValue>;
            
            data.Clear();
            foreach (var entry in dictionary.Items)
            {
                data.Add(new System.ValueTuple<TKey, TValue>(entry.Key, entry.Value));
            }
        }

        protected abstract TKey GetKey(TValue item);
        
        protected virtual void DrawEntry(TKey key, TValue value)
        {
            EditorGUILayout.LabelField(key.ToString(), value.ToString());
        }
    }
}