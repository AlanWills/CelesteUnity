using Celeste.Events;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Sound
{
    [Serializable]
    public class AudioClipUnityEvent : UnityEvent<AudioClip> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(AudioClipEvent), menuName = "Celeste/Events/Audio/Audio Clip Event")]
    public class AudioClipEvent : ParameterisedEvent<AudioClip>
    {
    }
}
