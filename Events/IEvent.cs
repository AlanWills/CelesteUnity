using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Events
{
    public interface IEvent
    {
        void AddEventListener(IEventListener listener);
        void RemoveEventListener(IEventListener listener);
        void Raise();
        void RaiseSilently();
    }

    public interface IEvent<T>
    {
        void AddEventListener(IEventListener<T> listener);
        void RemoveEventListener(IEventListener<T> listener);
        void Raise(T argument);
        void RaiseSilently(T argument);
    }
}
