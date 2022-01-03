using System.Collections;
using UnityEngine;

namespace Celeste.FSM
{
    public interface ILinearRuntimeRecord
    {
        FSMGraphNodePath CurrentNodePath { get; set; }
    }
}