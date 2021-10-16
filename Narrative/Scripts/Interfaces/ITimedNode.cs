using Celeste.Narrative.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Narrative
{
    public interface ITimedNode
    {
        float AllowedTime { get; }
        float ElapsedTime { get; }
    }
}
