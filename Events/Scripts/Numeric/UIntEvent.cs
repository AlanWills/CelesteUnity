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
    public class UIntUnityEvent : UnityEvent<uint> { }

    [Serializable]
    [CreateAssetMenu(fileName = "UIntEvent", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "UInt Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class UIntEvent : ParameterisedEvent<uint>
    {
    }
}
