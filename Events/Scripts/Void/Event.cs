using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    [CreateAssetMenu(fileName = "Event", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class Event : ScriptableObject, IEvent
    {
        #region Properties and Fields

#if UNITY_EDITOR
        [SerializeField, TextArea] private string helpText;
#endif

        private List<UnityActionCallback> gameEventListeners = new List<UnityActionCallback>();
        private List<UnityActionCallback> cachedListeners = new List<UnityActionCallback>();

        #endregion

        #region Event Management

        public ICallbackHandle AddListener(IEventListener listener)
        {
            return AddListener(listener.OnEventRaised);
        }

        public ICallbackHandle AddListener(UnityAction callback)
        {
            CallbackHandle callbackHandle = CallbackHandle.New();
            gameEventListeners.Add(new UnityActionCallback(callbackHandle, callback));

            return callbackHandle;
        }

        public void RemoveListener(IEventListener listener)
        {
            RemoveListener(listener.OnEventRaised);
        }

        public void RemoveListener(UnityAction callback)
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

        public void Invoke()
        {
            Invoke(true);
        }

        public void Invoke(bool logInvocation)
        {
            if (logInvocation)
            {
                Debug.Log($"Event {name} was raised", this);
            }

            InvokeSilently();
        }

        public void InvokeSilently()
        {
            // Do the check for gameEventListeners to ensure that if events are unsubscribed from a callback
            // we can handle that and don't fall over
            int gameEventListenersCount = gameEventListeners.Count;
            if (gameEventListenersCount > 0)
            {
                cachedListeners.Clear();
                cachedListeners.AddRange(gameEventListeners);

                // Cache the gameEventListeners to ensure that if events are unsubscribed from a callback
                // we can handle that and don't fall over
                for (int i = 0; i < gameEventListenersCount; ++i)
                {
                    Debug.Assert(cachedListeners[i].Action != null, $"Event {name} has a cached listener which is null.", this);
                    cachedListeners[i].Action();
                }

                cachedListeners.Clear();
            }
        }

        #endregion
    }
}
