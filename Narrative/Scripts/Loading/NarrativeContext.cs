using Celeste.Narrative.Persistence;
using Celeste.Scene.Events;
using System;
using System.Collections;
using UnityEngine;

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