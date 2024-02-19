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

        private NarrativeRuntime currentRuntime;

        #endregion

        private void BeginNarrativeRuntime(ChapterRecord chapterRecord)
        {
            chapterRecord.LoadChapterValueRecords();
            currentChapterRecord.Value = chapterRecord;

            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(transform);
            currentRuntime = NarrativeRuntime.Create(gameObject, chapterRecord);
            currentRuntime.OnNarrativeFinished.AddListener(OnNarrativeFinished);

            narrativeBegunEvent.Invoke(currentRuntime);
        }

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

        public void OnCalculateCurrentChapterProgress()
        {
            if (currentChapterRecord.Value != null)
            {
                currentChapterRecord.Value.CalculateProgress();
            }
        }

        private void OnNarrativeFinished()
        {
            narrativeFinishedEvent.Invoke(currentRuntime);
        }

        #endregion
    }
}
