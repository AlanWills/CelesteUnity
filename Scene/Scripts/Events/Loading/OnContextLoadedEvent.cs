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
    [CreateAssetMenu(fileName = "OnContextLoadedEvent", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Loading/On Context Loaded Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class OnContextLoadedEvent : ParameterisedEvent<OnContextLoadedArgs> 
    { 
        public void Invoke(Context context)
        {
            Invoke(new OnContextLoadedArgs(context));
        }
    }
}
