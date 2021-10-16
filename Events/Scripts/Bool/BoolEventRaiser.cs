using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Bool Event Raiser")]
    public class BoolEventRaiser : ParameterisedEventRaiser<bool, BoolEvent>, ISupportsValueArgument<bool, BoolValue>
    {
        public void Raise(BoolValue argument)
        {
            Raise(argument.Value);
        }
    }
}
