using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    public class ParameterisedEvent<T> : ScriptableObject, IEvent<T>
    {
        #region Properties and Fields

#if UNITY_EDITOR
        [SerializeField, TextArea] private string helpText;
#endif

        private List<UnityActionCallback<T>> gameEventListeners = new List<UnityActionCallback<T>>();

        #endregion

        #region Event Management

        public ICallbackHandle AddListener(IEventListener<T> listener)
        {
            return AddListener(listener.OnEventRaised);
        }

        public ICallbackHandle AddListener(UnityAction<T> callback)
        {
            ICallbackHandle callbackHandle = CallbackHandle.New();
            gameEventListeners.Add(new UnityActionCallback<T>(callbackHandle, callback));

            return callbackHandle;
        }

        public void RemoveListener(IEventListener<T> listener)
        {
            RemoveListener(listener.OnEventRaised);
        }

        public void RemoveListener(UnityAction<T> callback)
        {
            int callbackIndex = gameEventListeners.FindIndex(x => x.Action == callback);
            if (callbackIndex >= 0)
            {
                gameEventListeners.RemoveAt(callbackIndex);
            }
        }

        public void RemoveListener(ICallbackHandle callbackHandle)
        {
            int callbackIndex = gameEventListeners.FindIndex(x => x.Handle.Equals(callbackHandle));
            if (callbackIndex >= 0)
            {
                gameEventListeners.RemoveAt(callbackIndex);
            }
        }

        public void RemoveAllListeners()
        {
            gameEventListeners.Clear();
        }

        public void Invoke(T argument)
        {
            Invoke(argument, true);
        }

        public void Invoke(T argument, bool logInvocation)
        {
            if (logInvocation)
            {
                Debug.Log($"Event {name} was raised with argument {(argument != null ? argument.ToString() : "<null>")}", this);
            }

            InvokeSilently(argument);
        }

        public void InvokeSilently(T argument)
        {
            int gameEventListenersCount = gameEventListeners.Count;
            if (gameEventListenersCount > 0)
            {
                List<UnityActionCallback<T>> cachedListeners = new List<UnityActionCallback<T>>(gameEventListenersCount);
                cachedListeners.AddRange(gameEventListeners);

                // Cache the gameEventListeners to ensure that if events are unsubscribed from a callback
                // we can handle that and don't fall over
                for (int i = 0; i < gameEventListenersCount; ++i)
                {
                    Debug.Assert(cachedListeners[i].Action != null, $"Event {name} has a cached listener which is null.", this);
                    cachedListeners[i].Action(argument);
                }
            }
        }

        #endregion
    }
}
