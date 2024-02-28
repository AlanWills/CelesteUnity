using Celeste.Events;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Sound
{
    [Serializable]
    public class AudioClipUnityEvent : UnityEvent<AudioClip> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(AudioClipEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Audio/Audio Clip Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class AudioClipEvent : ParameterisedEvent<AudioClip>
    {
    }
}
