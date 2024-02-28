using Celeste.FSM;
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
    public class FSMRuntimeUnityEvent : UnityEvent<FSMRuntime> { }

    [Serializable]
    [CreateAssetMenu(fileName = "FSMRuntimeEvent", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "FSM/FSM Runtime Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class FSMRuntimeEvent : ParameterisedEvent<FSMRuntime>
    {
    }
}
