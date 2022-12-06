using Celeste.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Core
{
    [CreateAssetMenu(fileName = nameof(ScheduledCallbacks), menuName = "Celeste/Core/Scheduled Callbacks")]
    public class ScheduledCallbacks : ScriptableObject
    {
        #region Properties and Fields

        private struct ScheduledCallback
        {
            public long timestamp;
            public Action callback;
            public ICallbackHandle handle;

            public ScheduledCallback(long timestamp, Action callback, ICallbackHandle handle)
            {
                this.timestamp = timestamp;
                this.callback = callback;
                this.handle = handle;
            }
        }

        [NonSerialized] private List<ScheduledCallback> callbacks = new List<ScheduledCallback>();
        [NonSerialized] private List<ScheduledCallback> callbacksCopy = new List<ScheduledCallback>();
        [NonSerialized] private int latestHandleId = CallbackHandle.INVALID_HANDLE.Id;

        #endregion

        public CallbackHandle Schedule(long timestamp, Action callback)
        {
            CallbackHandle handle = new CallbackHandle(++latestHandleId);
            ScheduledCallback scheduledCallback = new ScheduledCallback(timestamp, callback, handle);
            callbacks.Add(scheduledCallback);

            return handle;
        }

        public void Cancel(ICallbackHandle callbackHandle)
        {
            int index = callbacks.FindIndex(x => x.handle == callbackHandle);
            if (index != -1)
            {
                callbacks.RemoveAt(index);
            }
        }

        public void Update()
        {
            callbacksCopy.Clear();

            if (callbacks.Count > 0)
            {
                callbacksCopy.AddRange(callbacks);
                
                for (int i = 0, n = callbacksCopy.Count; i < n; ++i)
                {
                    var scheduledCallback = callbacksCopy[i];

                    if (GameTime.Now >= scheduledCallback.timestamp)
                    {
                        scheduledCallback.callback();
                        
                        Cancel(scheduledCallback.handle);
                    }
                }
            }
        }
    }
}
