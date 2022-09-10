using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Vector/Vector3 Event Listener")]
    public class Vector3EventListener : ParameterisedEventListener<Vector3, Vector3Event, Vector3UnityEvent>
    {
    }
}
