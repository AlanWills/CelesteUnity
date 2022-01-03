using System.Collections;
using UnityEngine;

namespace Celeste.FSM
{
    public class FSMRecord : ILinearRuntimeRecord
    {
        public FSMGraphNodePath CurrentNodePath { get; set; } = new FSMGraphNodePath();
    }
}