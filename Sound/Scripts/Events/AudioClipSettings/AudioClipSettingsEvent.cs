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
    [CreateAssetMenu(fileName = nameof(AudioClipSettingsEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Audio/Audio Clip Settings Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class AudioClipSettingsEvent : ParameterisedEvent<AudioClipSettings>
    {
        public void Invoke(AudioClip audioClip, AudioChannelSettings channelSettings)
        {
            AudioClipSettings audioClipSettings = AudioClipSettings.Create(audioClip, channelSettings);
            Invoke(audioClipSettings);
        }
    }
}
