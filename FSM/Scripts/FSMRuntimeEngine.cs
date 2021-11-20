using System;
using UnityEngine;
using XNode;

namespace Celeste.FSM
{
    public class FSMRuntimeEngine
    {
        #region Properties and Fields

        private FSMNode CurrentNode 
        {
            set
            {
                if (runtime.CurrentNode != value)
                {
                    ExitCurrentNode();

                    runtime.CurrentNode = value;

                    EnterCurrentNode();
                }
            }
        }

        private ILinearRuntime<FSMNode> runtime;

        #endregion

        public FSMRuntimeEngine(ILinearRuntime<FSMNode> runtime)
        {
            this.runtime = runtime;
        }

        public void Start(FSMNode startNode)
        {
            CurrentNode = startNode;
            
            Debug.Log($"Spooling up FSM with starting node {startNode.name}");
        }

        public FSMNode Update()
        {
            FSMNode newNode = UpdateCurrentNode();

            while (newNode != runtime.CurrentNode)
            {
                CurrentNode = newNode;
                newNode = UpdateCurrentNode();
            }

            return runtime.CurrentNode;
        }

        #region Node Methods

        private void EnterCurrentNode()
        {
            if (runtime.CurrentNode != null)
            {
                runtime.CurrentNode.Enter();
                runtime.OnNodeEnter.Invoke(runtime.CurrentNode);
            }
        }

        private FSMNode UpdateCurrentNode()
        {
            if (runtime.CurrentNode != null)
            {
                FSMNode newNode = runtime.CurrentNode.Update();
                runtime.OnNodeUpdate.Invoke(runtime.CurrentNode);

                return newNode;
            }

            return runtime.CurrentNode;
        }

        private void ExitCurrentNode()
        {
            if (runtime.CurrentNode != null)
            {
                runtime.CurrentNode.Exit();
                runtime.OnNodeExit.Invoke(runtime.CurrentNode);
            }
        }

        #endregion
    }
}
