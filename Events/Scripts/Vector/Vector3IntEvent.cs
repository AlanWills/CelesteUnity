using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class Vector3IntUnityEvent : UnityEvent<Vector3Int> { }

    [Serializable]
    [CreateAssetMenu(fileName = "Vector3IntEvent", menuName = "Celeste/Events/Vector3Int/Vector3Int Event")]
    public class Vector3IntEvent : ParameterisedEvent<Vector3Int> { }
}
