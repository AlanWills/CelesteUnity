using Celeste.Events;
using Celeste.Scene.Events;
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
    [CreateNodeMenu("Celeste/Events/Listeners/On Context Loaded Event Listener")]
    [NodeTint(0.8f, 0.9f, 0)]
    public class OnContextLoadedEventListenerNode : ParameterisedEventListenerNode<OnContextLoadedArgs, OnContextLoadedEvent>
    {
    }
}
