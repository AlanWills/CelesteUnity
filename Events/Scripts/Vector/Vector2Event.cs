using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class Vector2UnityEvent : UnityEvent<Vector2> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(Vector2Event), menuName = "Celeste/Events/Vector2/Vector2 Event")]
    public class Vector2Event : ParameterisedEvent<Vector2> { }
}
