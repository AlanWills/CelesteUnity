using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Celeste.FSM
{
    [AddComponentMenu("Celeste/FSM/FSM Runtime")]
    public class FSMRuntime : SceneGraph<FSMGraph>, ILinearRuntime<FSMNode>
    {
        #region Properties and Fields

        [NonSerialized] private FSMNode currentNode;
        public FSMNode CurrentNode 
        {
            get { return currentNode; }
            set
            {
                if (currentNode != null)
                {
                    currentNode.Exit();
                }

                currentNode = value;

                if (currentNode != null)
                {
                    currentNode.Enter();
                }
            }
        }

        // Runtime only override of the start node - useful for loading an FSM at a particular state
        [NonSerialized] private FSMNode startNode;
        public FSMNode StartNode
        {
            get { return startNode != null ? startNode : graph.startNode; }
            set { startNode = value; }
        }

        [SerializeField]
        private bool lateUpdate = false;

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (graph == null)
            {
                return;
            }

            graph.Runtime = this;

            if (currentNode != null && currentNode != StartNode)
            {
                // Allows us to handle the FSM being restarting with one function and
                // help avoid people being caught out
                currentNode.Exit();
            }

            Debug.Assert(graph.nodes.Count == 0 || StartNode != null, "FSMRuntime graph contains nodes, but no start node is set so it will be disabled.");
            currentNode = StartNode;

            if (currentNode != null)
            {
                Debug.LogFormat("Spooling up FSM with starting node {0}", CurrentNode.name);
                currentNode.Enter();
            }
            else 
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (lateUpdate)
            {
                return;
            }

            UpdateImpl();
        }

        private void LateUpdate()
        {
            if (!lateUpdate)
            {
                return;
            }

            UpdateImpl();
        }

        #endregion

        private void UpdateImpl()
        {
            if (currentNode == null)
            {
                return;
            }

            FSMNode newNode = currentNode.Update();

            while (newNode != currentNode)
            {
                currentNode.Exit();
                currentNode = newNode;
                
                if (currentNode != null)
                {
                    currentNode.Enter();
                    newNode = currentNode.Update();
                }
            }
        }
    }
}
