using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Memory
{
    [AddComponentMenu("Celeste/Memory/Game Object Allocator")]
    public class GameObjectAllocator : MonoBehaviour, IEnumerable<GameObject>
    {
        #region Properties and Fields

        public int Available
        {
            get { return cache.Count - allocated; }
        }

        public GameObject prefab;

        [SerializeField, Min(1)]
        private uint defaultChunkSize = 5;

        [SerializeField, Min(0)]
        private uint allocateOnStart = 5;

        private List<GameObject> cache = new List<GameObject>();
        private int allocated = 0;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (prefab == null && transform.childCount > 0)
            {
                prefab = transform.GetChild(0).gameObject;
            }
        }

        private void Awake()
        {
            for (int i = cache.Count, n = transform.childCount; i < n; ++i)
            {
                cache.Add(transform.GetChild(i).gameObject);
            }

            EnsureCapacity(allocateOnStart);
        }

        #endregion

        #region Allocation Methods

        public void EnsureCapacity(uint capacity)
        {
            if (cache.Count < capacity)
            {
                AddChunk((uint)(capacity - cache.Count));
            }
        }

        public void AddChunk()
        {
            AddChunk(defaultChunkSize);
        }

        public void AddChunk(uint chunkSize)
        {
            cache.Capacity += (int)chunkSize;

            Debug.AssertFormat(prefab != null, "There is no prefab set on the GameObjectAllocator on GameObject {0}.  Please set one.", gameObject.name);
            for (uint i = 0; i < chunkSize; ++i)
            {
                GameObject instance = GameObject.Instantiate(prefab.gameObject, transform);
                instance.SetActive(false);
                cache.Add(instance);
            }
        }

        public GameObject AllocateWithResizeIfNecessary()
        {
            if (!CanAllocate(1))
            {
                AddChunk();
            }

            return Allocate();
        }

        public GameObject Allocate()
        {
            GameObject gameObject = FindInactiveGameObject();
#if ALLOCATOR_CHECKS
            if (gameObject == null)
            {
                Debug.LogAssertion("Invalid call to Allocate.  Dangerous side effects will occur here - ensure you call CanAllocate() first.");
                return null;
            }
#endif
            ++allocated;
            return gameObject;
        }

        public void Deallocate(GameObject gameObject)
        {
#if ALLOCATOR_CHECKS
            if (!cache.Contains(gameObject))
            {
                Debug.LogAssertionFormat("GameObject {0} is not from allocator {1}", gameObject.name, name);
                return;
            }
#endif
            --allocated;
            gameObject.SetActive(false);

            if (gameObject.transform.parent != transform)
            {
                gameObject.transform.SetParent(transform);
            }
        }

        public bool CanAllocate(uint requestedAmount)
        {
            return (uint)Available >= requestedAmount;
        }

        public void DeallocateAll()
        {
            for (int i = 0, n = cache.Count; i < n; ++i)
            {
                cache[i].SetActive(false);
            }

            allocated = 0;
        }

        private GameObject FindInactiveGameObject()
        {
            for (int i = 0, n = cache.Count; i < n; ++i)
            {
                if (!cache[i].activeSelf)
                {
                    return cache[i];
                }
            }

            return null;
        }

        #endregion

        #region IEnumerable

        public IEnumerator<GameObject> GetEnumerator()
        {
            for (int i = 0, n = cache.Count; i < n; ++i)
            {
                if (cache[i].activeSelf)
                {
                    yield return cache[i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
