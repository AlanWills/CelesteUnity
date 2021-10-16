using Celeste.DataStructures;
using System;
using System.Collections.Generic;

namespace Celeste.Narrative.Persistence
{
    public class ProductionRecord
    {
        #region Properties and Fields

        public int NumStoryRecords 
        { 
            get { return storyRecords.Count; } 
        }

        public int LastPlayedStoryGuid { get; set; }
        public int LastPlayedChapterGuid { get; set; }

        private List<StoryRecord> storyRecords = new List<StoryRecord>();

        #endregion

        public StoryRecord GetStoryRecord(int index)
        {
            return storyRecords.Get(index);
        }

        public StoryRecord AddStoryRecord(Story story)
        {
            StoryRecord storyRecord = new StoryRecord(story);
            storyRecords.Add(storyRecord);

            return storyRecord;
        }

        public StoryRecord FindOrAddStoryRecord(Story story)
        {
            StoryRecord storyRecord = storyRecords.Find(x => x.Story == story);
            if (storyRecord == null)
            {
                storyRecord = AddStoryRecord(story);
            }

            return storyRecord;
        }
    }
}