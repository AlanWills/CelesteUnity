using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Touch Event Listener")]
    public class TouchEventListener : ParameterisedEventListener<Touch, TouchEvent, TouchUnityEvent>
    {
    }
}
