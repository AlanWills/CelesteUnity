using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Game Object Event Listener")]
    public class GameObjectEventListener : ParameterisedEventListener<GameObject, GameObjectEvent, GameObjectUnityEvent>
    {
    }
}
