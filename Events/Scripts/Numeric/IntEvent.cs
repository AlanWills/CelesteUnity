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
    public class IntUnityEvent : UnityEvent<int> { }

    [Serializable]
    [CreateAssetMenu(fileName = "IntEvent", menuName = "Celeste/Events/Int Event")]
    public class IntEvent : ParameterisedEvent<int>
    {
    }
}
