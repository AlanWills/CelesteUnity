using Celeste.Events;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.FSM.Nodes.Events
{
    [Serializable]
    [CreateNodeMenu("Celeste/Events/Raisers/BoolEvent Raiser")]
    public class BoolEventRaiserNode : ParameterisedEventRaiserNode<bool, BoolValue, BoolReference, BoolEvent>
    {
    }
}
