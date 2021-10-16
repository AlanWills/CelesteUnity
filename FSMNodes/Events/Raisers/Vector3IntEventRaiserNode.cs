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
    [CreateNodeMenu("Celeste/Events/Raisers/Vector3IntEvent Raiser")]
    public class Vector3IntEventRaiserNode : ParameterisedEventRaiserNode<Vector3Int, Vector3IntValue, Vector3IntReference, Vector3IntEvent>
    {
    }
}
