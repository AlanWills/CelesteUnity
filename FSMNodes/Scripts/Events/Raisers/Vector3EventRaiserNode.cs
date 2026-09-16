using Celeste.Events;
using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.FSM.Nodes.Events
{
    [Serializable]
    [CreateNodeMenu("Celeste/Events/Raisers/Vector3 Event Raiser")]
    public class Vector3EventRaiserNode : ParameterisedEventRaiserNode<Vector3, Vector3Value, Vector3Reference, Vector3Event>
    {
    }
}
