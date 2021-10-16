using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Bool Event Listener")]
    public class BoolEventListener : ParameterisedEventListener<bool, BoolEvent, BoolUnityEvent>
    {
    }
}
