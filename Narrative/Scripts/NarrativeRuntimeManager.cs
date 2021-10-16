using Celeste.Narrative.Events;
using Celeste.Narrative.Loading;
using Celeste.Narrative.Parameters;
using Celeste.Narrative.Persistence;
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

        private NarrativeRuntime currentRuntime;

        #endregion

        private void BeginNarrativeRuntime(ChapterRecord chapterRecord)
        {
            if (chapterRecord == null)
            {
                UnityEngine.Debug.LogAssertion("Failed to begin narrative runtime due to null chapter record.");
                return;
            }

            currentChapterRecord.Value = chapterRecord;

            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(transform);
            currentRuntime = NarrativeRuntime.Create(gameObject, chapterRecord);

            narrativeBegunEvent.Invoke(currentRuntime);
        }

        #region Callbacks

        public void OnNarrativeContextLoaded(OnContextLoadedArgs onContextLoadedArgs)
        {
            NarrativeContext narrativeContext = onContextLoadedArgs.context as NarrativeContext;
            BeginNarrativeRuntime(narrativeContext.chapterRecord);
        }

        public void OnCalculateCurrentChapterProgress()
        {
            if (currentChapterRecord.Value != null)
            {
                currentChapterRecord.Value.CalculateProgress();
            }
        }

        #endregion
    }
}
