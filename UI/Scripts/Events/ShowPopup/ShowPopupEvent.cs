using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class ShowPopupArgs { }

    [Serializable]
    public class ShowPopupUnityEvent : UnityEvent<ShowPopupArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(ShowPopupEvent), menuName = "Celeste/Events/UI/Show Popup Event")]
    public class ShowPopupEvent : ParameterisedEvent<ShowPopupArgs> { }
}
