using Celeste.Narrative;
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
    public class StoryUnityEvent : UnityEvent<Story> { }

    [Serializable]
    [CreateAssetMenu(fileName = "StoryEvent", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Story Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class StoryEvent : ParameterisedEvent<Story>
    {
    }
}
