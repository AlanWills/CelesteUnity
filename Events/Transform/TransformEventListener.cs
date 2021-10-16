using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Transform Event Listener")]
    public class TransformEventListener : ParameterisedEventListener<Transform, TransformEvent, TransformUnityEvent>
    {
    }
}
