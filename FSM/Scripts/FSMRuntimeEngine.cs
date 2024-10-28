using UnityEngine;

namespace Celeste.FSM
{
    public class FSMRuntimeEngine
    {
        #region Properties and Fields

        public FSMNode CurrentNode 
        {
            get => currentNode;
            set
            {
                if (currentNode != value)
                {
                    ExitCurrentNode();

                    currentNode = value;

                    EnterCurrentNode();
                }
            }
        }

        private ILinearRuntime runtime;
        private FSMNode currentNode;

        #endregion

        public FSMRuntimeEngine(ILinearRuntime runtime)
        {
            this.runtime = runtime;
        }

        public void Start(FSMNode startNode)
        {
            CurrentNode = startNode;
            
            Debug.Log($"Spooling up FSM with starting node {(startNode != null ? startNode.name : "none")}");
        }

        public bool Update()
        {
            FSMNode newNode = UpdateCurrentNode();

            while (newNode != currentNode)
            {
                CurrentNode = newNode;
                newNode = UpdateCurrentNode();
            }

            return currentNode != null;
        }

        #region Node Methods

        private void EnterCurrentNode()
        {
            if (currentNode != null)
            {
                currentNode.Enter();
                runtime.OnNodeEnter.Invoke(currentNode);
            }
        }

        private FSMNode UpdateCurrentNode()
        {
            if (currentNode != null)
            {
                FSMNode newNode = currentNode.Update();
                runtime.OnNodeUpdate.Invoke(currentNode);

                return newNode;
            }

            return currentNode;
        }

        private void ExitCurrentNode()
        {
            if (currentNode != null)
            {
                currentNode.Exit();
                runtime.OnNodeExit.Invoke(currentNode);
            }
        }

        #endregion
    }
}
