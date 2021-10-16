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
    public class BoolUnityEvent : UnityEvent<bool> { }

    [Serializable]
    [CreateAssetMenu(fileName = "BoolEvent", menuName = "Celeste/Events/Bool Event")]
    public class BoolEvent : ParameterisedEvent<bool>
    {
    }
}
