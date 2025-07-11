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
    public class ChapterUnityEvent : UnityEvent<Chapter> { }

    [Serializable]
    [CreateAssetMenu(fileName = "ChapterEvent", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Narrative/Chapter Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class ChapterEvent : ParameterisedEvent<Chapter>
    {
    }
}
