using Celeste.Events;
using Celeste.Sound.Settings;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Sound
{
    [Serializable]
    public class AudioClipSettingsUnityEvent : UnityEvent<AudioClipSettings> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(AudioClipSettingsEvent), menuName = "Celeste/Events/Audio/Audio Clip Settings Event")]
    public class AudioClipSettingsEvent : ParameterisedEvent<AudioClipSettings>
    {
        public void Invoke(AudioClip audioClip, AudioChannelSettings channelSettings)
        {
            AudioClipSettings audioClipSettings = AudioClipSettings.Create(audioClip, channelSettings);
            Invoke(audioClipSettings);
        }
    }
}
