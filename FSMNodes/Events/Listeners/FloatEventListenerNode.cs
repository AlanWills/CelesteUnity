using Celeste.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Celeste.FSM.Nodes.Events
{
    [Serializable]
    [CreateNodeMenu("Celeste/Events/Listeners/FloatEvent Listener")]
    [NodeTint(0.8f, 0.9f, 0)]
    public class FloatEventListenerNode : ParameterisedEventListenerNode<float, FloatEvent>
    {
    }
}
