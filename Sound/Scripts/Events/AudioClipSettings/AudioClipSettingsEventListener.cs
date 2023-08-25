using Celeste.Events;
using Celeste.Sound.Settings;
using UnityEngine;

namespace Celeste.Sound
{
    [AddComponentMenu("Celeste/Events/Audio/Audio Clip Settings Event Listener")]
    public class AudioClipSettingsEventListener : ParameterisedEventListener<AudioClipSettings, AudioClipSettingsEvent, AudioClipSettingsUnityEvent>
    {
    }
}
