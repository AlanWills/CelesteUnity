using Celeste.Events;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Loading.Events
{
    [Serializable]
    public class LoadJobUnityEvent : UnityEvent<LoadJob> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(LoadJobEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Loading/Load Job Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class LoadJobEvent : ParameterisedEvent<LoadJob>
    {
    }
}
