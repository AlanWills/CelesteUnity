using UnityEngine.Events;

namespace Celeste.Events
{
    public interface IEvent
    {
        void AddListener(IEventListener listener);
        void AddListener(UnityAction callback);

        void RemoveListener(IEventListener listener);
        void RemoveListener(UnityAction callback);

        void Invoke();
        void InvokeSilently();
    }

    public interface IEvent<T>
    {
        void AddListener(IEventListener<T> listener);
        void AddListener(UnityAction<T> callback);

        void RemoveListener(IEventListener<T> listener);
        void RemoveListener(UnityAction<T> callback);

        void Invoke(T argument);
        void InvokeSilently(T argument);
    }
}
