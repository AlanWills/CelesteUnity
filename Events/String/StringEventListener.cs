using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/String Event Listener")]
    public class StringEventListener : ParameterisedEventListener<string, StringEvent, StringUnityEvent>
    {
    }
}
