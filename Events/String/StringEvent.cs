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
    public class StringUnityEvent : UnityEvent<string> { }

    [Serializable]
    [CreateAssetMenu(fileName = "StringEvent", menuName = "Celeste/Events/String Event")]
    public class StringEvent : ParameterisedEvent<string>
    {
    }
}
