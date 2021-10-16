using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class AudioClipUnityEvent : UnityEvent<AudioClip> { }

    [Serializable]
    [CreateAssetMenu(fileName = "AudioClipEvent", menuName = "Celeste/Events/Audio/Audio Clip Event")]
    public class AudioClipEvent : ParameterisedEvent<AudioClip>
    {
    }
}
