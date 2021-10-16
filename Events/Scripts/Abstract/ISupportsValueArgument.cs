using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Events
{
    public interface ISupportsValueArgument<T, TValue> where TValue : ParameterValue<T>
    {
        void Raise(TValue argument);
    }
}
