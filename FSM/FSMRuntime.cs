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
    public class FSMRuntime : SceneGraph<FSMGraph>
    {
        #region Properties and Fields

        public FSMNode CurrentNode { get; private set; }

        [SerializeField]
        private bool lateUpdate = false;

        #endregion

        #region Unity Methods

        public void Start()
        {
            if (graph == null)
            {
                return;
            }

            if (CurrentNode != null)
            {
                // Allows us to handle the FSM being restarting with one function and
                // help avoid people being caught out
                CurrentNode.Exit();
            }

            Debug.Assert(graph.nodes.Count == 0 || graph.startNode != null, "FSMRuntime graph contains nodes, but no start node so will be disabled.");
            CurrentNode = graph.startNode;

            if (CurrentNode != null)
            {
                Debug.LogFormat("Spooling up FSM with starting node {0}", CurrentNode.name);
                CurrentNode.Enter();
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
            if (CurrentNode == null)
            {
                return;
            }

            FSMNode newNode = CurrentNode.Update();

            while (newNode != CurrentNode)
            {
                CurrentNode.Exit();
                CurrentNode = newNode;

                if (CurrentNode != null)
                {
                    CurrentNode.Enter();
                    newNode = CurrentNode.Update();
                }
            }
        }
    }
}
