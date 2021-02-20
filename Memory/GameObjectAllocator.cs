using Celeste.Attributes.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Memory
{
    [AddComponentMenu("Celeste/Memory/Game Object Allocator")]
    public class GameObjectAllocator : MonoBehaviour
    {
        #region Properties and Fields

        public GameObject prefab;

        [SerializeField, Min(1), Tooltip("The number of GameObjects that will be allocated when this allocator runs out of space.")]
        private uint chunkSize = 5;

        private List<GameObject> cache = new List<GameObject>();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (cache.Capacity == 0)
            {
                AddChunk();
            }
        }

        #endregion

        #region Allocation Methods

        public void AddChunk()
        {
            cache.Capacity += (int)chunkSize;

            Debug.AssertFormat(prefab != null, "There is no prefab set on the GameObjectAllocator on GameObject {0}.  Please set one.", gameObject.name);
            for (uint i = 0; i < chunkSize; ++i)
            {
                GameObject instance = GameObject.Instantiate(prefab, transform);
                instance.SetActive(false);
                cache.Add(instance);
            }
        }

        public GameObject Allocate()
        {
            GameObject gameObject = FindInactiveGameObject();
            Debug.Assert(gameObject != null, "Invalid call to Allocate.  Dangerous side effects will occur here - ensure you call CanAllocate() first.");
            gameObject.SetActive(true);
            
            return gameObject;
        }

        public bool CanAllocate(uint requestedAmount)
        {
            uint currentCount = 0;

            foreach (GameObject gameObject in cache)
            {
                currentCount = gameObject.activeSelf ? currentCount : currentCount + 1;
            }

            return currentCount >= requestedAmount;
        }

        public void DeallocateAll()
        {
            foreach (GameObject gameObject in cache)
            {
                gameObject.SetActive(false);
            }
        }

        private GameObject FindInactiveGameObject()
        {
            foreach (GameObject gameObject in cache)
            {
                if (!gameObject.activeSelf)
                {
                    return gameObject;
                }
            }

            return null;
        }

        #endregion
    }
}
