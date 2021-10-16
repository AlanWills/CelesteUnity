using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Int Event Raiser")]
    public class IntEventRaiser : ParameterisedEventRaiser<int, IntEvent>, ISupportsValueArgument<int, IntValue>
    {
        public void Raise(IntValue argument)
        {
            Raise(argument.Value);
        }
    }
}
