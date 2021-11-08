using Celeste.Narrative;
using Celeste.Narrative.Characters;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class BackgroundUnityEvent : UnityEvent<Background> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(BackgroundEvent), menuName = "Celeste/Events/Background Event")]
    public class BackgroundEvent : ParameterisedEvent<Background>
    {
    }
}
