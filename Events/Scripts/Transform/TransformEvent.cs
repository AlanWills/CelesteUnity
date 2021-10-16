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
    public class TransformUnityEvent : UnityEvent<Transform> { }

    [Serializable]
    [CreateAssetMenu(fileName = "TransformEvent", menuName = "Celeste/Events/Transform Event")]
    public class TransformEvent : ParameterisedEvent<Transform>
    {
    }
}
