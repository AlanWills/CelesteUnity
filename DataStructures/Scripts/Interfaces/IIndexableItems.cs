using System;
using System.Collections.Generic;

namespace Celeste.DataStructures
{
    public interface IIndexableItems<T>
    {
        int NumItems { get; }
        IReadOnlyList<T> Items { get; }

        T GetItem(int index);
        T FindItem(Predicate<T> predicate);
        void AddItem(T item);
        void RemoveItem(int index);

        void ClearDuplicates();
    }
}