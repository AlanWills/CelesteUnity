using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Int Event Listener")]
    public class IntEventListener : ParameterisedEventListener<int, IntEvent, IntUnityEvent>
    {
    }
}
