using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Memory
{
    [AddComponentMenu("Celeste/Memory/Game Object Allocator (Stack)")]
    public class GameObjectAllocator_Stack : MonoBehaviour, IEnumerable<GameObject>
    {
        #region Properties and Fields

        public uint Capacity => capacity;
        public uint Available => (uint)available.Count;

        public GameObject prefab;

        [SerializeField, Min(1)]
        private uint defaultChunkSize = 5;

        [SerializeField, Min(0)]
        private uint allocateOnStart = 5;

        private readonly Stack<GameObject> available = new();
        private readonly List<GameObject> allocated = new();
        private uint capacity = 0;

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
            EnsureCapacity(allocateOnStart);
        }

        #endregion

        #region Allocation Methods

        public void EnsureCapacity(uint desiredCapacity)
        {
            if (capacity < desiredCapacity)
            {
                AddChunk(desiredCapacity - capacity);
            }
        }

        public void EnsureCapacity(int capacity)
        {
            EnsureCapacity((uint)capacity);
        }

        public void AddChunk()
        {
            AddChunk(defaultChunkSize);
        }

        public void AddChunk(uint chunkSize)
        {
            capacity += chunkSize;

            Debug.Assert(prefab != null, $"There is no prefab set on the GameObjectAllocator on GameObject {gameObject.name}!  Please set one...");
            for (uint i = 0; i < chunkSize; ++i)
            {
                GameObject instance = GameObject.Instantiate(prefab.gameObject, transform);
                instance.SetActive(false);
                available.Push(instance);
            }
        }

        public GameObject AllocateWithResizeIfNecessary(bool shouldAllocateDisabled = false)
        {
            if (!CanAllocate(1))
            {
                AddChunk();
            }

            return Allocate(shouldAllocateDisabled);
        }

        public GameObject Allocate(bool shouldAllocateDisabled = false)
        {
            bool popSuccessful = available.TryPop(out GameObject instance);
#if ALLOCATOR_CHECKS
            if (!popSuccessful)
            {
                Debug.LogAssertion($"Invalid call to {nameof(Allocate)}.  Dangerous side effects will occur here - ensure you call {nameof(CanAllocate)} first.");
                return null;
            }
#endif
            instance.SetActive(!shouldAllocateDisabled);
            return instance;
        }

        public void Deallocate(GameObject instance)
        {
#if ALLOCATOR_CHECKS
            if (!allocated.Contains(instance))
            {
                Debug.LogAssertion($"{nameof(GameObject)} {instance} is not from allocator {name}!");
                return;
            }
#endif
            instance.SetActive(false);

            if (instance.transform.parent != transform)
            {
                instance.transform.SetParent(transform);
            }
            
            available.Push(instance);
            allocated.Remove(instance);
        }

        public bool CanAllocate(uint requestedAmount)
        {
            return Available >= requestedAmount;
        }

        public void DeallocateAll()
        {
            while (allocated.Count > 0)
            {
                Deallocate(allocated[0]);
            }
        }

        #endregion

        #region IEnumerable

        public IEnumerator<GameObject> GetEnumerator()
        {
            return allocated.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
