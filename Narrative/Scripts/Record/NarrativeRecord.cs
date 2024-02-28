using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = nameof(NarrativeRecord), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Narrative Record", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class NarrativeRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumStoryRecords 
        { 
            get { return storyRecords.Count; } 
        }

        public int LastPlayedStoryGuid { get; set; }
        public int LastPlayedChapterGuid { get; set; }
        
        [NonSerialized] private List<StoryRecord> storyRecords = new List<StoryRecord>();

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

        public ChapterRecord FindOrAddChapterRecord(Story story, Chapter chapter)
        {
            StoryRecord storyRecord = FindOrAddStoryRecord(story);
            return storyRecord.FindOrAddChapterRecord(chapter);
        }

        public ChapterRecord FindLastPlayedChapterRecord(StoryCatalogue storyCatalogue)
        {
            Story story = storyCatalogue.FindByGuid(LastPlayedStoryGuid);
            if (story == null)
            {
                return null;
            }

            Chapter chapter = story.FindChapter(LastPlayedChapterGuid);
            if (chapter == null)
            {
                return null;
            }

            return FindOrAddChapterRecord(story, chapter);
        }
    }
}