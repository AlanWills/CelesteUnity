using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Game Object Event Raiser")]
    public class GameObjectEventRaiser : ParameterisedEventRaiser<GameObject, GameObjectEvent>
    {
    }
}
