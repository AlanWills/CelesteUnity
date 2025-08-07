using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Events
{
    public interface IEventRaiser
    {
        void Raise();
    }

    public interface IEventRaiser<in T>
    {
        void Raise(T argument);
    }
}
