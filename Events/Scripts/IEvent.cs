using UnityEngine.Events;

namespace Celeste.Events
{
    public interface IReadOnlyEvent
    {
        ICallbackHandle AddListener(IEventListener listener);
        ICallbackHandle AddListener(UnityAction callback);

        void RemoveListener(IEventListener listener);
        void RemoveListener(UnityAction callback);
        void RemoveListener(ICallbackHandle callbackHandle);

    }
    
    public interface IReadOnlyEvent<T>
    {
        ICallbackHandle AddListener(IEventListener<T> listener);
        ICallbackHandle AddListener(UnityAction<T> callback);

        void RemoveListener(IEventListener<T> listener);
        void RemoveListener(UnityAction<T> callback);
        void RemoveListener(ICallbackHandle callbackHandle);
    }

    public interface IEvent : IReadOnlyEvent
    {
        void Invoke();
        void InvokeSilently();
    }

    public interface IEvent<T> : IReadOnlyEvent<T>
    {
        void Invoke(T argument);
        void InvokeSilently(T argument);
    }
}
