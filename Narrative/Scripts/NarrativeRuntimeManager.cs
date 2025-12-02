using Celeste.Events;
using Celeste.FSM;
using Celeste.Narrative.Events;
using Celeste.Narrative.Loading;
using Celeste.Narrative.Parameters;
using Celeste.Scene.Events;
using UnityEngine;

namespace Celeste.Narrative
{
    [AddComponentMenu("Celeste/Narrative/Narrative Runtime Manager")]
    public class NarrativeRuntimeManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ChapterRecordValue currentChapterRecord;
        [SerializeField] private NarrativeRuntimeEvent narrativeBegunEvent;
        [SerializeField] private NarrativeRuntimeEvent narrativeFinishedEvent;
        [SerializeField] private FSMNodeEvent narrativeNodeEnter;
        [SerializeField] private FSMNodeEvent narrativeNodeUpdate;
        [SerializeField] private FSMNodeEvent narrativeNodeExit;

        private NarrativeRuntime currentRuntime;

        #endregion

        private void BeginNarrativeRuntime(ChapterRecord chapterRecord)
        {
            chapterRecord.LoadChapterValueRecords();
            currentChapterRecord.Value = chapterRecord;

            GameObject narrativeRuntimeGameObject = new GameObject();
            narrativeRuntimeGameObject.transform.SetParent(transform);
            currentRuntime = NarrativeRuntime.Create(narrativeRuntimeGameObject, chapterRecord);
            currentRuntime.OnStop.AddListener(OnNarrativeFinished);
            currentRuntime.OnNodeEnter.AddListener(OnNarrativeNodeEnter);
            currentRuntime.OnNodeUpdate.AddListener(OnNarrativeNodeUpdate);
            currentRuntime.OnNodeExit.AddListener(OnNarrativeNodeExit);

            narrativeBegunEvent.Invoke(currentRuntime);
        }
        
        #region Debug Callbacks

        public void DebugNext()
        {
            if (currentRuntime == null)
            {
                return;
            }
            
            currentRuntime.TryGoToNextDefaultNode();
        }

        public void DebugRestart()
        {
            if (currentRuntime == null)
            {
                return;
            }

            currentRuntime.JumpTo(currentRuntime.StartNode);
        }
        
        public void DebugFinish()
        {
            if (currentRuntime == null)
            {
                return;
            }

            currentRuntime.JumpTo(currentRuntime.FinishNode);
        }
        
        #endregion

        #region Callbacks

        public void OnNarrativeContextLoaded(OnContextLoadedArgs onContextLoadedArgs)
        {
            NarrativeContext narrativeContext = onContextLoadedArgs.context as NarrativeContext;
            ChapterRecord chapterRecord = narrativeContext.chapterRecord;

            if (chapterRecord == null)
            {
                UnityEngine.Debug.LogAssertion("Encountered error beginning narrative runtime due to null chapter record.");
                return;
            }

            BeginNarrativeRuntime(narrativeContext.chapterRecord);
        }

        public void OnSetBackground(SetBackgroundEventArgs backgroundEventArgs)
        {
            if (currentRuntime != null)
            {
                currentRuntime.ChapterRecord.CurrentBackgroundGuid = backgroundEventArgs.Background != null ? backgroundEventArgs.Background.Guid : 0;
            }
        }

        public void OnCalculateCurrentChapterProgress()
        {
            if (currentChapterRecord.Value != null)
            {
                currentChapterRecord.Value.CalculateProgress();
            }
        }

        public void OnTryGoToNextDefaultNode()
        {
            if (currentRuntime != null)
            {
                currentRuntime.TryGoToNextDefaultNode();
            }
        }

        private void OnNarrativeFinished(ILinearRuntime linearRuntime)
        {
            narrativeFinishedEvent.Invoke(currentRuntime);
        }

        private void OnNarrativeNodeEnter(FSMNode node)
        {
            narrativeNodeEnter.Invoke(node);
        }

        private void OnNarrativeNodeUpdate(FSMNode node)
        {
            narrativeNodeUpdate.Invoke(node);
        }

        private void OnNarrativeNodeExit(FSMNode node)
        {
            narrativeNodeExit.Invoke(node);
        }

        #endregion
    }
}
