using Celeste.FSM;
using Celeste.Narrative.Loading;
using Celeste.Scene.Events;
using System;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace Celeste.Narrative
{
    [AddComponentMenu("Celeste/Narrative/Narrative Runtime")]
    public class NarrativeRuntime : SceneGraph<NarrativeGraph>, ILinearRuntime
    {
        #region Properties and Fields

        ILinearRuntimeRecord ILinearRuntime.Record => Record;

        public UnityEvent<ILinearRuntime> OnRun => onRun;
        public UnityEvent<ILinearRuntime> OnStop => onStop;
        public FSMNodeUnityEvent OnNodeEnter => onNodeEnter;
        public FSMNodeUnityEvent OnNodeUpdate => onNodeUpdate;
        public FSMNodeUnityEvent OnNodeExit => onNodeExit;

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

        [SerializeField] private UnityEvent<ILinearRuntime> onRun = new UnityEvent<ILinearRuntime>();
        [SerializeField] private UnityEvent<ILinearRuntime> onStop = new UnityEvent<ILinearRuntime>();
        [SerializeField] private FSMNodeUnityEvent onNodeEnter = new FSMNodeUnityEvent();
        [SerializeField] private FSMNodeUnityEvent onNodeUpdate = new FSMNodeUnityEvent();
        [SerializeField] private FSMNodeUnityEvent onNodeExit = new FSMNodeUnityEvent();
        [SerializeField] private bool startAutomatically = true;
        [SerializeField] private bool lateUpdate = false;

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
            runtime.SetupGraphAtRuntime(chapterRecord);

            return runtime;
        }

        public static NarrativeRuntime Create(
            GameObject gameObject,
            NarrativeGraph narrativeGraph)
        {
            gameObject.name = nameof(NarrativeRuntime);

            NarrativeRuntime runtime = gameObject.AddComponent<NarrativeRuntime>();
            runtime.SetupGraphAtRuntime(narrativeGraph);

            return runtime;
        }

        #endregion

        public void Run()
        {
            currentNode = null;
            enabled = true;

            if (graph != null)
            {
                graph.Runtime = this;
                OnRun.Invoke(this);

                runtimeEngine = new FSMRuntimeEngine(this);
                runtimeEngine.Start(StartNode);
            }

            if (CurrentNode == null)
            {
                Stop();
            }
        }

        public void Stop()
        {
            OnStop.Invoke(this);

            enabled = false;
            runtimeEngine = null;
            currentNode = null;
        }

        private void UpdateFSM()
        {
            if (runtimeEngine == null)
            {
                return;
            }    

            if (CurrentNode == null)
            {
                Stop();
            }
            else
            {
                CurrentNode = runtimeEngine.Update();
            }
        }

        private void SetupGraphAtRuntime(NarrativeGraph narrativeGraph)
        {
            graph = narrativeGraph;
            graph.Runtime = this;
        }

        private void SetupGraphAtRuntime(ChapterRecord chapter)
        {
            graph = chapter.Chapter.NarrativeGraph;
            graph.Runtime = this;
            Record = chapter;
            StartNode = chapter.CurrentNodePath.Node;
        }

        #region Unity Methods

        private void Start()
        {
            if (startAutomatically == false)
            {
                return;
            }

            Run();
        }

        private void Update()
        {
            if (!lateUpdate)
            {
                UpdateFSM();
            }
        }

        private void LateUpdate()
        {
            if (lateUpdate)
            {
                UpdateFSM();
            }
        }

        #endregion

        #region Callbacks

        public void OnNarrativeContextLoaded(OnContextLoadedArgs args)
        {
            NarrativeContext narrativeContext = args.context as NarrativeContext;
            SetupGraphAtRuntime(narrativeContext.chapterRecord.Chapter.NarrativeGraph);
            Run();
        }

        #endregion
    }
}