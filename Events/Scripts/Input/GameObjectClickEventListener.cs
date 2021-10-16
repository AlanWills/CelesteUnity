using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Game Object Click Event Listener")]
    public class GameObjectClickEventListener : ParameterisedEventListener<GameObjectClickEventArgs, GameObjectClickEvent, GameObjectClickUnityEvent>
    {
    }
}
