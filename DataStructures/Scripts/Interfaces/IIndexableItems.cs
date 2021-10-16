using System;
using System.Collections;
using UnityEngine;

namespace Celeste.DataStructures
{
    public interface IIndexableItems<T>
    {
        int NumItems { get; }

        T GetItem(int index);
        T FindItem(Predicate<T> predicate);
    }
}