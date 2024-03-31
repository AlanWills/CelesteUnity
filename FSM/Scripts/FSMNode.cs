using Celeste.Objects;
using System;
using UnityEngine;
using XNode;
using XNode.Attributes;

namespace Celeste.FSM
{
    [Serializable]
    public abstract class FSMNode : Node
    {
        #region Properties and Fields

        public const string DEFAULT_INPUT_PORT_NAME = " ";
        public const string DEFAULT_OUTPUT_PORT_NAME = "";

        public FSMGraph FSMGraph => graph as FSMGraph;
        public string Guid => guid;

        [SerializeField, HideInNodeEditor] private string guid;

        #endregion

        public FSMNode()
        {
            AddDefaultInputPort();
            AddDefaultOutputPort();
        }

        #region FSM Runtime Methods

        protected override void Init()
        {
            base.Init();

            hideFlags = HideFlags.HideInHierarchy;

            if (string.IsNullOrEmpty(guid))
            {
                guid = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public void Enter() 
        { 
            OnEnter(); 
        }

        public FSMNode Update()
        {
            return OnUpdate();
        }

        public void Exit() 
        {
            OnExit();
        }

        protected virtual void OnEnter() { }

        protected virtual FSMNode OnUpdate() { return GetConnectedNodeFromDefaultOutput(); }

        protected virtual void OnExit() { }

        #endregion

        #region FSM Utility Methods

        protected void AddDefaultInputPort(ConnectionType connectionType = ConnectionType.Multiple)
        {
            AddInputPort(DEFAULT_INPUT_PORT_NAME, connectionType);
        }

        protected void RemoveDefaultInputPort()
        {
            RemoveDynamicPort(DEFAULT_INPUT_PORT_NAME);
        }

        protected void AddDefaultOutputPort(ConnectionType connectionType = ConnectionType.Override)
        {
            AddOutputPort(DEFAULT_OUTPUT_PORT_NAME, connectionType);
        }

        protected void RemoveDefaultOutputPort()
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

        protected FSMNode GetConnectedNode(string outputPortName)
        {
            NodePort nodePort = GetOutputPort(outputPortName);
            if (nodePort == null || nodePort.ConnectionCount == 0)
            {
                return null;
            }

            NodePort connection = nodePort.GetConnection(0);
            return connection != null ? connection.node as FSMNode : null;
        }

        public FSMNode GetConnectedNodeFromDefaultOutput()
        {
            return GetConnectedNode(DEFAULT_OUTPUT_PORT_NAME);
        }

        public NodePort GetDefaultOutputPort()
        {
            return GetOutputPort(DEFAULT_OUTPUT_PORT_NAME);
        }

        public NodePort GetDefaultInputPort()
        {
            return GetInputPort(DEFAULT_INPUT_PORT_NAME);
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

        public void CopyInGraph(FSMNode original) 
        {
            OnCopyInGraph(original);
        }

        protected virtual void OnCopyInGraph(FSMNode original) { }

        #endregion

        #region Parameter Methods

        protected T CreateParameter<T>(string name) where T : ScriptableObject
        {
            return (graph as FSMGraph).CreateParameter<T>(name);
        }

        protected T CreateParameter<T>(T original) where T : ScriptableObject, ICopyable<T>
        {
            return (graph as FSMGraph).CreateParameter(original);
        }

        protected void RemoveParameter<T>(T parameter) where T : ScriptableObject
        {
            (graph as FSMGraph).RemoveAsset(parameter);
        }

        #endregion

        #region Context Menu

        [ContextMenu("Set As Start")]
        public void SetAsStart()
        {
            (graph as FSMGraph).startNode = this;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(graph);
#endif
        }

        [ContextMenu("Set As Finish")]
        public void SetAsFinish()
        {
            if (graph is IProgressFSMGraph progressGraph)
            {
                progressGraph.FinishNode = this;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(graph);
#endif
            }
        }

        #endregion
    }
}
