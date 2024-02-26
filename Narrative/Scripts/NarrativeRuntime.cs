using Celeste.FSM;
using System;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace Celeste.Narrative
{
    [AddComponentMenu("Celeste/Narrative/Narrative Runtime")]
    public class NarrativeRuntime : SceneGraph<NarrativeGraph>, ILinearRuntime<FSMNode>
    {
        #region Properties and Fields

        ILinearRuntimeRecord ILinearRuntime<FSMNode>.Record => Record;

        public FSMNodeUnityEvent OnNodeEnter => onNodeEnter;
        public FSMNodeUnityEvent OnNodeUpdate => onNodeUpdate;
        public FSMNodeUnityEvent OnNodeExit => onNodeExit;
        public UnityEvent OnNarrativeFinished => onNarrativeFinished;

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

        [SerializeField] private bool startAutomatically = true;
        [SerializeField] private FSMNodeUnityEvent onNodeEnter = new FSMNodeUnityEvent();
        [SerializeField] private FSMNodeUnityEvent onNodeUpdate = new FSMNodeUnityEvent();
        [SerializeField] private FSMNodeUnityEvent onNodeExit = new FSMNodeUnityEvent();
        [SerializeField] private UnityEvent onNarrativeFinished = new UnityEvent();

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

        public static NarrativeRuntime Create(
            GameObject gameObject,
            NarrativeGraph narrativeGraph)
        {
            gameObject.name = nameof(NarrativeRuntime);

            NarrativeRuntime runtime = gameObject.AddComponent<NarrativeRuntime>();
            runtime.graph = narrativeGraph;

            narrativeGraph.Runtime = runtime;

            return runtime;
        }

        #endregion

        public void StartNarrative()
        {
            currentNode = null;
            enabled = true;

            if (graph != null)
            {
                graph.Runtime = this;

                runtimeEngine = new FSMRuntimeEngine(this);
                runtimeEngine.Start(StartNode);
            }

            if (CurrentNode == null)
            {
                StopNarrative();
            }
        }

        public void StopNarrative()
        {
            enabled = false;
            runtimeEngine = null;
            currentNode = null;
        }

        #region Unity Methods

        private void Start()
        {
            if (startAutomatically == false)
            {
                return;
            }

            StartNarrative();
        }

        private void Update()
        {
            if (runtimeEngine != null && runtimeEngine.Update() == graph.finishNode)
            {
                OnNarrativeFinished.Invoke();
                StopNarrative();
            }
        }

        #endregion
    }
}