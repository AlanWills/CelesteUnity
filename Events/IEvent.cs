using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Events
{
    public interface IEvent
    {
        void AddListener(IEventListener listener);
        void AddListener(Action callback);

        void RemoveListener(IEventListener listener);
        void RemoveListener(Action callback);

        void Invoke();
        void InvokeSilently();
    }

    public interface IEvent<T>
    {
        void AddListener(IEventListener<T> listener);
        void AddListener(Action<T> callback);

        void RemoveListener(IEventListener<T> listener);
        void RemoveListener(Action<T> callback);

        void Invoke(T argument);
        void InvokeSilently(T argument);
    }
}
