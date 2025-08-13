using Celeste.Narrative.Backgrounds;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public struct SetBackgroundEventArgs : IEventArgs
    {
        public Background Background;
        public float Offset;
    }
    
    [Serializable]
    public class SetBackgroundUnityEvent : UnityEvent<SetBackgroundEventArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(SetBackgroundEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Narrative/Set Background Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class SetBackgroundEvent : ParameterisedEvent<SetBackgroundEventArgs>
    {
    }
}
