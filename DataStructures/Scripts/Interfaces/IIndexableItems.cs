using System;

namespace Celeste.DataStructures
{
    public interface IIndexableItems<T>
    {
        int NumItems { get; }

        T GetItem(int index);
        T FindItem(Predicate<T> predicate);
    }
}