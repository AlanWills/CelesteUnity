﻿namespace Celeste.FSM
{
    public interface ILinearRuntime
    {
        public FSMNodeUnityEvent OnNodeEnter { get; }
        public FSMNodeUnityEvent OnNodeUpdate { get; }
        public FSMNodeUnityEvent OnNodeExit { get; }

        FSMNode StartNode { get; }
        FSMNode CurrentNode { get; }
        ILinearRuntimeRecord Record { get; }
    }
}