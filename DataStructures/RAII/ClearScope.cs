using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DataStructures
{
    public struct ClearScope : IDisposable
    {
        private IList list;

        public ClearScope(IList list)
        {
            this.list = list;
            list.Clear();
        }

        public void Dispose()
        {
            list.Clear();
        }
    }
}