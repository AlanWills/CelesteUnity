using Celeste.Events;
using Celeste.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Narrative.Events
{
    [Serializable]
    public class NarrativeRuntimeUnityEvent : UnityEvent<NarrativeRuntime> { }

    [Serializable]
    [CreateAssetMenu(fileName = "NarrativeRuntimeEvent", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Narrative/Narrative Runtime Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class NarrativeRuntimeEvent : ParameterisedEvent<NarrativeRuntime>
    {
    }
}
