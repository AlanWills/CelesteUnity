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
    [CreateAssetMenu(fileName = "NarrativeRuntimeEvent", menuName = "Celeste/Events/Narrative/Narrative Runtime Event")]
    public class NarrativeRuntimeEvent : ParameterisedEvent<NarrativeRuntime>
    {
    }
}
