using System.Collections;
using UnityEngine;

namespace Celeste.FSM
{
    public class FSMRecord : ILinearRuntimeRecord
    {
        public string CurrentNodeGuid { get; set; }
        public string CurrentSubGraphNodeGuid { get; set; }
    }
}