using Celeste.Events;
using Celeste.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Narrative.Events
{
    [AddComponentMenu("Celeste/Events/Narrative/Narrative Runtime Event Listener")]
    public class NarrativeRuntimeEventListener : ParameterisedEventListener<NarrativeRuntime, NarrativeRuntimeEvent, NarrativeRuntimeUnityEvent>
    {
    }
}
