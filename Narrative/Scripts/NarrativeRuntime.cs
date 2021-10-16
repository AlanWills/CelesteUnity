using Celeste.FSM;
using Celeste.Narrative.Parameters;
using Celeste.Narrative.Persistence;
using System;
using System.Collections;
using UnityEngine;
using XNode;

namespace Celeste.Narrative
{
    [AddComponentMenu("Celeste/Narrative/Narrative Runtime")]
    public class NarrativeRuntime : SceneGraph<NarrativeGraph>, ILinearRuntime<FSMNode>
    {
        #region Properties and Fields

        public FSMNodeUnityEvent OnNodeEnter { get; } = new FSMNodeUnityEvent();
        public FSMNodeUnityEvent OnNodeUpdate { get; } = new FSMNodeUnityEvent();
        public FSMNodeUnityEvent OnNodeExit { get; } = new FSMNodeUnityEvent();

        public ChapterRecord Record { get; set; }

        [NonSerialized] private FSMNode doNotAssignDirectly_currentNode;
        public FSMNode CurrentNode 
        {
            get { return doNotAssignDirectly_currentNode; }
            private set
            {
                doNotAssignDirectly_currentNode = value;

                if (Record != null)
                {
                    Record.CurrentNodeGuid = value != null ? value.Guid.ToString() : "";
                }
            }
        }

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
            runtime.CurrentNode = narrativeGraph.nodes.Find(x => string.CompareOrdinal((x as FSMNode).Guid, chapterRecord.CurrentNodeGuid) == 0) as FSMNode;

            narrativeGraph.Runtime = runtime;
            
            return runtime;
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (graph == null)
            {
                return;
            }

            if (CurrentNode == null)
            {
                CurrentNode = graph.startNode;
            }

            if (CurrentNode != null)
            {
                UnityEngine.Debug.LogFormat("Spooling up FSM with starting node {0}", CurrentNode.name);
                EnterCurrentNode();
            }
            else
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (CurrentNode == null)
            {
                return;
            }

            FSMNode newNode = UpdateCurrentNode();

            while (newNode != CurrentNode)
            {
                ExitCurrentNode();
                CurrentNode = newNode;

                if (CurrentNode != null)
                {
                    EnterCurrentNode();
                    newNode = UpdateCurrentNode();
                }
            }
        }

        #endregion

        #region Node Methods

        private void EnterCurrentNode()
        {
            CurrentNode.Enter();
            OnNodeEnter.Invoke(CurrentNode);
        }

        private FSMNode UpdateCurrentNode()
        {
            FSMNode newNode = CurrentNode.Update();
            OnNodeUpdate.Invoke(CurrentNode);

            return newNode;
        }

        private void ExitCurrentNode()
        {
            CurrentNode.Exit();
            OnNodeExit.Invoke(CurrentNode);
        }

        #endregion
    }
}