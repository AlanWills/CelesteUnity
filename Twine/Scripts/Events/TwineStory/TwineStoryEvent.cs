using Celeste.Twine;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class TwineStoryUnityEvent : UnityEvent<TwineStory> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(TwineStoryEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Twine/Twine Story Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class TwineStoryEvent : ParameterisedEvent<TwineStory>
    {
    }
}
