using Celeste.Twine;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class TwineStoryUnityEvent : UnityEvent<TwineStory> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(TwineStoryEvent), menuName = "Celeste/Events/Twine/Twine Story Event")]
    public class TwineStoryEvent : ParameterisedEvent<TwineStory>
    {
    }
}
