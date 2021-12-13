using Celeste.Scene.Events;
using System;

namespace Celeste.Narrative.Loading
{
    [Serializable]
    public class NarrativeContext : Context
    {
        public ChapterRecord chapterRecord;

        public NarrativeContext(ChapterRecord chapterRecord)
        {
            this.chapterRecord = chapterRecord;
        }
    }
}