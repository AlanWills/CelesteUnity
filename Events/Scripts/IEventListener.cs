using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Events
{
    public interface IEventListener
    {
        void OnEventRaised();
    }

    public interface IEventListener<T>
    {
        void OnEventRaised(T arguments);
    }
}
