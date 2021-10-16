using Celeste.FSM;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/FSM/FSM Runtime Event Raiser")]
    public class FSMRuntimeEventRaiser : ParameterisedEventRaiser<FSMRuntime, FSMRuntimeEvent>
    {
    }
}
