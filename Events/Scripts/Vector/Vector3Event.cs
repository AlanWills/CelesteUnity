using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class Vector3UnityEvent : UnityEvent<Vector3> { }

    [Serializable]
    [CreateAssetMenu(fileName = "Vector3Event", menuName = "Celeste/Events/Vector3 Event")]
    public class Vector3Event : ParameterisedEvent<Vector3> { }
}
