using Celeste.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/FSM/FSM Runtime Event Listener")]
    public class FSMRuntimeEventListener : ParameterisedEventListener<FSMRuntime, FSMRuntimeEvent, FSMRuntimeUnityEvent>
    {
    }
}
