using Celeste.UI.Skin;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class UISkinUnityEvent : UnityEvent<UISkin> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(UISkinEvent), menuName = "Celeste/Events/UI/UI Skin Event")]
    public class UISkinEvent : ParameterisedEvent<UISkin> { }
}
