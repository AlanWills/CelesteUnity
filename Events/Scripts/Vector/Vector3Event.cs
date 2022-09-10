using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class Vector3UnityEvent : UnityEvent<Vector3> { }

    [Serializable]
    [CreateAssetMenu(fileName = "Vector3Event", menuName = "Celeste/Events/Vector3/Vector3 Event")]
    public class Vector3Event : ParameterisedEvent<Vector3> { }
}
