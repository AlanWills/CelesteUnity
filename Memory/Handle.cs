using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Memory
{
    public class Handle<T>
    {
        public T item;
        public bool isAllocated;

        public Handle(T item, bool isAllocated)
        {
            this.item = item;
            this.isAllocated = isAllocated;
        }
    }
}
