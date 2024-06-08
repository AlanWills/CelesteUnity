using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Objects
{
    public class DictionaryScriptableObject<TKey, TValue> : ScriptableObject, IEnumerable<KeyValuePair<TKey, TValue>>, ISerializationCallbackReceiver
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

        #region Item

        [Serializable]
        private struct Item
        {
            public TKey key;
            public TValue value;
        }

        #endregion

        #region Properties and Fields

        public int NumItems => ItemsImpl.Count;
        public IReadOnlyDictionary<TKey, TValue> Items => itemsDictionary;
        private Dictionary<TKey, TValue> ItemsImpl => itemsDictionary;

        [SerializeField] private List<Item> items = new List<Item>();

        [NonSerialized] private Dictionary<TKey, TValue> itemsDictionary = new Dictionary<TKey, TValue>();

        #endregion

        public bool HasItem(TKey key)
        {
            return ItemsImpl.ContainsKey(key);
        }

        public TValue GetItem(TKey key)
        {
            return ItemsImpl.TryGetValue(key, out TValue value) ? value : default;
        }

        public bool TryFindItem(TKey key, out TValue value)
        {
            return ItemsImpl.TryGetValue(key, out value);
        }

        public void AddItem(TKey key, TValue value)
        {
            using (new ChangeBlock(this))
            {
#if KEY_CHECKS
                if (ItemsImpl.ContainsKey(key))
                {
                    UnityEngine.Debug.LogAssertion($"{name} already contains key {key}.");
                    return;
                }
#endif
                ItemsImpl.Add(key, value);
            }
        }

        public bool RemoveItem(TKey key)
        {
            using (new ChangeBlock(this))
            {
                return ItemsImpl.Remove(key);
            }
        }

        public void SetItems(IReadOnlyDictionary<TKey, TValue> newItems)
        {
            using (new ChangeBlock(this))
            {
                ItemsImpl.Clear();
                
                foreach (var item in newItems)
                {
                    ItemsImpl.Add(item.Key, item.Value);
                }
            }
        }

        public void Clear()
        {
            using (new ChangeBlock(this))
            {
                ItemsImpl.Clear();
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)ItemsImpl).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)ItemsImpl).GetEnumerator();
        }

        private void OnPreModify()
        {
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

        #region ISerializationCallbackReceiver

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            items.Clear();

            foreach (var keyValuePair in itemsDictionary)
            {
                items.Add(new Item() { key = keyValuePair.Key, value = keyValuePair.Value });
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            itemsDictionary.Clear();
            itemsDictionary.EnsureCapacity(items.Count);

            foreach (var item in items)
            {
#if KEY_CHECKS
                if (itemsDictionary.TryGetValue(item.key, out TValue oldValue))
                {
                    Debug.LogAssertion($"{nameof(DictionaryScriptableObject<TKey, TValue>)} already contains an item with key {item.key}.  The old value {oldValue} will be replaced with {item.value}.");
                }
#endif
                itemsDictionary[item.key] = item.value;
            }
        }

        #endregion
    }
}
