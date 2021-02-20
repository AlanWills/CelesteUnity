using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Debugging.Commands
{
    public interface IDebugCommand
    {
        bool Execute(List<string> parameters, StringBuilder output);
    }
}
