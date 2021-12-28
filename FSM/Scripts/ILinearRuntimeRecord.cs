using System.Collections;
using UnityEngine;

namespace Celeste.FSM
{
    public interface ILinearRuntimeRecord
    {
        string CurrentNodeGuid { get; set; }
        string CurrentSubGraphNodeGuid { get; set; }
    }
}