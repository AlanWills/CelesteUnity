using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Memory
{
    public class Cache<T> where T : IDisposable, new()
    {
        #region Properties and Fields

        private List<Handle<T>> items = new List<Handle<T>>();

        #endregion

        public Cache(int startingCapacity)
        {
            AddItems(startingCapacity);
        }

        #region Allocation

        public T Allocate()
        {
            int availableItemIndex = FindFirstAvailableItemIndex();
            if (availableItemIndex < 0)
            {
                // Set this before we add items otherwise we'll get the adjusted Count
                availableItemIndex = items.Count;
                AddItems(items.Capacity);
            }

            Handle<T> handle = items[availableItemIndex];
            handle.isAllocated = true;

            return handle.item;
        }

        public void Deallocate(T item)
        {
            int index = FindIndexOf(item);
            Debug.AssertFormat(index >= 0, "Item {0} is not from Cache", item);

            if (index >= 0)
            {
                DeallocateImpl(index);
            }
        }

        public void DeallocateAll()
        {
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i].isAllocated)
                {
                    DeallocateImpl(i);
                }
            }
        }

        public void DeallocateAllBut(T item)
        {
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i].isAllocated && !items[i].item.Equals(item))
                {
                    DeallocateImpl(i);
                }
            }
        }

        private void DeallocateImpl(int index)
        {
            Debug.AssertFormat(0 <= index && index < items.Count, "Index {0} is out of bounds", index);
            items[index].item.Dispose();
            items[index].isAllocated = false;
        }

        private void AddItems(int extraItems)
        {
            items.Capacity += extraItems;

            for (int i = 0; i < extraItems; ++i)
            {
                items.Add(new Handle<T>(new T(), false));
            }
        }

        private int FindFirstAvailableItemIndex()
        {
            for (int i = 0; i < items.Count; ++i)
            {
                if (!items[i].isAllocated)
                {
                    return i;
                }
            }

            return -1;
        }

        private int FindIndexOf(T item)
        {
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i].item.Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion
    }
}
