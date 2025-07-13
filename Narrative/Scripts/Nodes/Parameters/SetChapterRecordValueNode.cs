using Celeste.FSM;
using UnityEngine;

namespace Celeste.Narrative.Parameters
{
    [CreateNodeMenu("Celeste/Parameters/Narrative/Set Chapter Record Value")]
    [NodeWidth(250)]
    public class SetChapterRecordValueNode : FSMNode
    {
        #region Properties and Fields

        [SerializeField] private ChapterRecordValue chapterRecordValue;
        [SerializeField] private NarrativeRecord narrativeRecord;
        [SerializeField] private Story newChaptersStory;
        [SerializeField] private Chapter newChapterValue;

        #endregion
        
        protected override void OnEnter()
        {
            base.OnEnter();

            chapterRecordValue.Value = narrativeRecord.FindOrAddChapterRecord(newChaptersStory, newChapterValue);
        }
    }
}