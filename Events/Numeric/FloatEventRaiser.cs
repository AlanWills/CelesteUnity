using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Float Event Raiser")]
    public class FloatEventRaiser : ParameterisedEventRaiser<float, FloatEvent>, ISupportsValueArgument<float, FloatValue>
    {
        public void Raise(FloatValue argument)
        {
            Raise(argument.Value);
        }
    }
}
