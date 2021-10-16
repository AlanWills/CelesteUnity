using Celeste.FSM.Nodes.Loading;
using Celeste.Narrative.Loading;
using Celeste.Narrative.Parameters;
using Celeste.Narrative.Persistence;
using Celeste.Scene.Events;
using System;
using static XNode.Node;

namespace Celeste.Narrative
{
    [Serializable]
    [CreateNodeMenu("Celeste/Narrative/Load Chapter")]
    public class LoadChapterNode : LoadContextNode
    {
        public ChapterRecordValue currentChapterRecord;

        protected override Context CreateContext()
        {
            ChapterRecord record = currentChapterRecord.Value;
            return record != null ? new NarrativeContext(record) : null;
        }
    }
}
