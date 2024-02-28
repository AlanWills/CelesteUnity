using Celeste.UI.Skin;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class UISkinUnityEvent : UnityEvent<UISkin> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(UISkinEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "UI/UI Skin Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class UISkinEvent : ParameterisedEvent<UISkin> { }
}
