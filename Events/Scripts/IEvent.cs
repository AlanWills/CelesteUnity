using UnityEngine.Events;

namespace Celeste.Events
{
    public interface IEvent
    {
        ICallbackHandle AddListener(IEventListener listener);
        ICallbackHandle AddListener(UnityAction callback);

        void RemoveListener(IEventListener listener);
        void RemoveListener(UnityAction callback);
        void RemoveListener(ICallbackHandle callbackHandle);

        void Invoke();
        void InvokeSilently();
    }

    public interface IEvent<T>
    {
        ICallbackHandle AddListener(IEventListener<T> listener);
        ICallbackHandle AddListener(UnityAction<T> callback);

        void RemoveListener(IEventListener<T> listener);
        void RemoveListener(UnityAction<T> callback);
        void RemoveListener(ICallbackHandle callbackHandle);

        void Invoke(T argument);
        void InvokeSilently(T argument);
    }
}
