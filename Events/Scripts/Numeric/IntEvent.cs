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
    public class IntUnityEvent : UnityEvent<int> { }

    [Serializable]
    [CreateAssetMenu(fileName = "IntEvent", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Int Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class IntEvent : ParameterisedEvent<int>
    {
    }
}
