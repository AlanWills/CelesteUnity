using Celeste.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.FSM.Nodes.Events.Conditions
{
    [Serializable]
    [DisplayName("String")]
    public class StringEventCondition : ParameterizedEventCondition<string, StringEvent>
    {
    }
}
