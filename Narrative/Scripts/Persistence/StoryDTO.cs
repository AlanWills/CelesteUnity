using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.Persistence
{
    [Serializable]
    public struct StoryDTO
    {
        public int guid;
        public List<ChapterDTO> chapters;

        public StoryDTO(StoryRecord storyRecord)
        {
            guid = storyRecord.Story.Guid;
            chapters = new List<ChapterDTO>(storyRecord.NumChapterRecords);

            for (int i = 0, n = storyRecord.NumChapterRecords; i < n; ++i)
            {
                chapters.Add(new ChapterDTO(storyRecord.GetChapterRecord(i)));
            }
        }
    }
}