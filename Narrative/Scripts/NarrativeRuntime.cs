using Celeste.FSM;
using Celeste.Narrative.Loading;
using Celeste.Scene.Events;
using UnityEngine;

namespace Celeste.Narrative
{
    [AddComponentMenu("Celeste/Narrative/Narrative Runtime")]
    public class NarrativeRuntime : FSMRuntime
    {
        public ChapterRecord ChapterRecord => record as ChapterRecord;
        public FSMNode FinishNode => (graph as NarrativeGraph).FinishNode;

        #region Factory Functions

        public static NarrativeRuntime Create(
            GameObject gameObject,
            ChapterRecord chapterRecord)
        {
            gameObject.name = nameof(NarrativeRuntime);
            
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

        private void SetupGraphAtRuntime(NarrativeGraph narrativeGraph)
        {
            graph = narrativeGraph;
            graph.Runtime = this;
        }

        private void SetupGraphAtRuntime(ChapterRecord chapter)
        {
            graph = chapter.Chapter.NarrativeGraph;
            graph.Runtime = this;
            record = chapter;
            StartNode = chapter.CurrentNodePath.Node;
        }

        #region Callbacks

        public void OnNarrativeContextLoaded(OnContextLoadedArgs args)
        {
            UnityEngine.Debug.Log($"On Narrative Context Loaded received from {name}.");
            NarrativeContext narrativeContext = args.context as NarrativeContext;
            SetupGraphAtRuntime(narrativeContext.chapterRecord.Chapter.NarrativeGraph);
            Run();
        }

        #endregion
    }
}