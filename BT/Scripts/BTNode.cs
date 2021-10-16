using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Celeste.BT
{
    [Serializable]
    public abstract class BTNode : Node
    {
        #region Properties and Fields

        public const string DEFAULT_INPUT_PORT_NAME = " ";
        public const string DEFAULT_OUTPUT_PORT_NAME = "";

        #endregion

        public BTNode()
        {
            AddDefaultInputPort();
            AddDefaultOutputPort();
        }

        #region Runtime Methods

        protected override void Init()
        {
            base.Init();

            hideFlags = HideFlags.HideInHierarchy;
        }

        public BTNode Evaluate(BTBlackboard btBlackboard)
        {
            return OnEvaluate(btBlackboard);
        }

        protected virtual BTNode OnEvaluate(BTBlackboard btBlackboard) { return this; }

        #endregion

        #region Node Utility Methods

        protected void AddDefaultInputPort(ConnectionType connectionType = ConnectionType.Multiple)
        {
            AddInputPort(DEFAULT_INPUT_PORT_NAME, connectionType);
        }

        protected void AddDefaultOutputPort(ConnectionType connectionType = ConnectionType.Override)
        {
            AddOutputPort(DEFAULT_OUTPUT_PORT_NAME, connectionType);
        }

        protected void RemoveDefaultOutputPort(ConnectionType connectionType = ConnectionType.Override)
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);
        }

        public void AddInputPort(string portName, ConnectionType connectionType = ConnectionType.Multiple)
        {
            AddDynamicInput(typeof(void), connectionType, TypeConstraint.None, portName);
        }

        public void AddOutputPort(string portName, ConnectionType connectionType = ConnectionType.Override)
        {
            AddDynamicOutput(typeof(void), connectionType, TypeConstraint.None, portName);
        }

        protected BTNode GetConnectedNode(string outputPortName)
        {
            NodePort nodePort = GetOutputPort(outputPortName);
            if (nodePort == null || nodePort.ConnectionCount == 0)
            {
                return null;
            }

            NodePort connection = nodePort.GetConnection(0);
            return connection != null ? connection.node as BTNode : null;
        }

        protected BTNode GetDefaultOutputConnectedNode()
        {
            return GetConnectedNode(DEFAULT_OUTPUT_PORT_NAME);
        }

        #endregion

        #region Add/Remove/Copy

        public void AddToGraph()
        {
            OnAddToGraph();
        }

        protected virtual void OnAddToGraph() { }

        public void RemoveFromGraph()
        {
            OnRemoveFromGraph();
        }

        protected virtual void OnRemoveFromGraph() { }

        public void CopyInGraph(BTNode original)
        {
            OnCopyInGraph(original);
        }

        protected virtual void OnCopyInGraph(BTNode original) { }

        #endregion

        #region Context Menu

        [ContextMenu("Set As Start")]
        public void SetAsStart()
        {
            (graph as BTGraph).startNode = this;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(graph);
#endif
        }

        #endregion
    }
}
