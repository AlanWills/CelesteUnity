using Celeste.DataStructures;
using Celeste.OdinSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Celeste.Objects
{
    public class DictionaryScriptableObject<TKey, TValue> : SerializedScriptableObject, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        #region Change Block

        private struct ChangeBlock : IDisposable
        {
            private DictionaryScriptableObject<TKey, TValue> dictionarySO;

            public ChangeBlock(DictionaryScriptableObject<TKey, TValue> dictionarySO)
            {
                this.dictionarySO = dictionarySO;
                dictionarySO.OnPreModify();
            }

            public void Dispose()
            {
                dictionarySO.OnPostModify();
            }
        }

        #endregion

        #region Properties and Fields

        public int NumItems { get { return ItemsImpl.Count; } }

        private Dictionary<TKey, TValue> ItemsImpl => runtimeModifiedItems != null ? runtimeModifiedItems : items;

        [SerializeField] private Dictionary<TKey, TValue> items = new Dictionary<TKey, TValue>();

        [NonSerialized] private Dictionary<TKey, TValue> runtimeModifiedItems;

        #endregion

        public TValue GetItem(TKey key)
        {
            return ItemsImpl.TryGetValue(key, out TValue value) ? value : default;
        }

        public void AddItem(TKey key, TValue value)
        {
            using (ChangeBlock changeBlock = new ChangeBlock(this))
            {
                ItemsImpl.Add(key, value);
            }
        }

        public void SetItems(IReadOnlyDictionary<TKey, TValue> newItems)
        {
            using (ChangeBlock changeBlock = new ChangeBlock(this))
            {
                ItemsImpl.Clear();
                
                foreach (var item in newItems)
                {
                    ItemsImpl.Add(item.Key, item.Value);
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }

        private void OnPreModify()
        {
            if (Application.isPlaying && runtimeModifiedItems == null)
            {
                // We've wanted to modify items for the first time at runtime so we create a copy of our serialized list
                // to prevent any runtime changes affecting what will be serialized and saved
                runtimeModifiedItems = new Dictionary<TKey, TValue>(items);
            }
        }

        private void OnPostModify()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }
    }
}
