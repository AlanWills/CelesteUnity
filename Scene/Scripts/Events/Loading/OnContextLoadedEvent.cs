using Celeste.Events;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Scene.Events
{
    [Serializable]
    public struct OnContextLoadedArgs
    {
        public Context context;

        public OnContextLoadedArgs(Context context)
        {
            this.context = context;
        }
    }

    [Serializable]
    public class OnContextLoadedUnityEvent : UnityEvent<OnContextLoadedArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = "OnContextLoadedEvent", menuName = "Celeste/Events/Loading/On Context Loaded Event")]
    public class OnContextLoadedEvent : ParameterisedEvent<OnContextLoadedArgs> 
    { 
        public void Invoke(Context context)
        {
            Invoke(new OnContextLoadedArgs(context));
        }
    }
}
