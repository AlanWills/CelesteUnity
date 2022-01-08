using Celeste.Localisation;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class LocalisationKeyUnityEvent : UnityEvent<LocalisationKey> { }

    [CreateAssetMenu(fileName = nameof(LocalisationKeyEvent), menuName = "Celeste/Events/Localisation/Localisation Key Event")]
    public class LocalisationKeyEvent : ParameterisedEvent<LocalisationKey>
    {
    }
}
