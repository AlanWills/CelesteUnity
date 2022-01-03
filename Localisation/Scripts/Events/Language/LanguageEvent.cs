using Celeste.Localisation;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class LanguageUnityEvent : UnityEvent<Language> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(LanguageEvent), menuName = "Celeste/Events/Localisation/Language Event")]
    public class LanguageEvent : ParameterisedEvent<Language>
    {
    }
}
