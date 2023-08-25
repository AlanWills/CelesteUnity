using Celeste.Events;
using UnityEngine;

namespace Celeste.Sound
{
    [AddComponentMenu("Celeste/Events/Audio/Audio Clip Event Listener")]
    public class AudioClipEventListener : ParameterisedEventListener<AudioClip, AudioClipEvent, AudioClipUnityEvent>
    {
    }
}
