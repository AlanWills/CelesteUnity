using Celeste.FSM;
using Celeste.FSM.Nodes;
using Celeste.Narrative.Persistence;
using System;
using UnityEngine;
using XNode;

namespace Celeste.Narrative
{
    [AddComponentMenu("Celeste/Narrative/Narrative Runtime")]
    public class NarrativeRuntime : SceneGraph<NarrativeGraph>, ILinearRuntime<FSMNode>
    {
        #region Properties and Fields

        ILinearRuntimeRecord ILinearRuntime<FSMNode>.Record => Record;

        public FSMNodeUnityEvent OnNodeEnter { get; } = new FSMNodeUnityEvent();
        public FSMNodeUnityEvent OnNodeUpdate { get; } = new FSMNodeUnityEvent();
        public FSMNodeUnityEvent OnNodeExit { get; } = new FSMNodeUnityEvent();

        public ChapterRecord Record { get; set; }

        [NonSerialized] private FSMNode currentNode;
        public FSMNode CurrentNode 
        {
            get { return currentNode; }
            set
            {
                if (currentNode != value)
                {
                    currentNode = value;

                    if (Record != null && currentNode != null)
                    {
                        Record.CurrentNodePath = new FSMGraphNodePath(currentNode);
                    }
                }
            }
        }

        [NonSerialized] private FSMNode startNode;
        public FSMNode StartNode
        {
            get { return startNode != null ? startNode : graph.startNode; }
            set { startNode = value; }
        }

        private FSMRuntimeEngine runtimeEngine;

        #endregion

        #region Factory Functions

        public static NarrativeRuntime Create(
            GameObject gameObject,
            ChapterRecord chapterRecord)
        {
            gameObject.name = nameof(NarrativeRuntime);

            NarrativeGraph narrativeGraph = chapterRecord.Chapter.NarrativeGraph;
            NarrativeRuntime runtime = gameObject.AddComponent<NarrativeRuntime>();
            runtime.graph = narrativeGraph;
            runtime.Record = chapterRecord;
            runtime.StartNode = chapterRecord.CurrentNodePath.Node;

            narrativeGraph.Runtime = runtime;
            
            return runtime;
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (graph != null)
            {
                graph.Runtime = this;

                runtimeEngine = new FSMRuntimeEngine(this);
                runtimeEngine.Start(StartNode);
            }

            if (CurrentNode != null)
            {
                UnityEngine.Debug.LogFormat("Spooling up FSM with starting node {0}", CurrentNode.name);
            }
            else
            {
                enabled = false;
            }
        }

        private void Update()
        {
            runtimeEngine.Update();
        }

        #endregion
    }
}