using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Vector3Int Event Listener")]
    public class Vector3IntEventListener : ParameterisedEventListener<Vector3Int, Vector3IntEvent, Vector3IntUnityEvent>
    {
    }
}
