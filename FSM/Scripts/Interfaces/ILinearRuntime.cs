using System.Collections;
using UnityEngine;

namespace Celeste.FSM
{
    public interface ILinearRuntime<TNode>
    {
        public FSMNodeUnityEvent OnNodeEnter { get; }
        public FSMNodeUnityEvent OnNodeUpdate { get; }
        public FSMNodeUnityEvent OnNodeExit { get; }

        TNode StartNode { get; }
        TNode CurrentNode { get; set; }
        ILinearRuntimeRecord Record { get; }
    }
}