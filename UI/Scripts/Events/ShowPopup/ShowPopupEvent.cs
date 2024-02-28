using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    public interface IPopupArgs { }

    [Serializable]
    public struct NoPopupArgs : IPopupArgs { }

    [Serializable]
    public class ShowPopupUnityEvent : UnityEvent<IPopupArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(ShowPopupEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "UI/Show Popup Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class ShowPopupEvent : ParameterisedEvent<IPopupArgs> 
    {
        public void InvokeNoArgs()
        {
            Invoke(new NoPopupArgs());
        }
    }
}
