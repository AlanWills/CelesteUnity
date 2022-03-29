using Celeste.DataStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative
{
    public class StoryRecord
    {
        #region Properties and Fields

        public Story Story { get; }
        
        public string StoryName
        {
            get { return Story.StoryName; }
        }

        public string StoryDescription
        {
            get { return Story.StoryDescription; }
        }

        public Sprite StoryThumbnail
        {
            get { return Story.StoryThumbnail; }
        }

        public int ChaptersComplete
        {
            get
            {
                int chaptersComplete = 0;
                for (int i = 0, n = chapterRecords.Count; i < n; ++i)
                {
                    if (chapterRecords[i].Progress >= 1)
                    {
                        ++chaptersComplete;
                    }
                }

                return chaptersComplete;
            }
        }

        public int ChaptersInStory
        {
            get { return Story.NumChapters; }
        }

        public int NumChapterRecords
        {
            get { return chapterRecords.Count; }
        }

        private List<ChapterRecord> chapterRecords = new List<ChapterRecord>();

        #endregion

        public StoryRecord(Story story)
        {
            Story = story;
        }

        public ChapterRecord GetChapterRecord(int index)
        {
            return chapterRecords.Get(index);
        }

        public ChapterRecord AddChapterRecord(
            Chapter chapter, 
            string currentNodePath)
        {
            ChapterRecord chapterRecord = new ChapterRecord(this, chapter, currentNodePath);
            chapterRecords.Add(chapterRecord);

            return chapterRecord;
        }

        public ChapterRecord FindOrAddChapterRecord(Chapter chapter)
        {
            ChapterRecord chapterRecord = chapterRecords.Find(x => x.Chapter == chapter);
            if (chapterRecord == null)
            {
                chapterRecord = AddChapterRecord(chapter, string.Empty);
            }

            return chapterRecord;
        }
    }
}