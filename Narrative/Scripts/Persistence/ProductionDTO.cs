using Celeste.Persistence;
using System;
using System.Collections.Generic;

namespace Celeste.Narrative.Persistence
{
    [Serializable]
    public class ProductionDTO : VersionedDTO
    {
        public int lastPlayedStoryGuid;
        public int lastPlayedChapterGuid;
        public List<StoryDTO> stories = new List<StoryDTO>();

        public ProductionDTO() { }

        public ProductionDTO(NarrativeRecord productionRecord)
        {
            lastPlayedStoryGuid = productionRecord.LastPlayedStoryGuid;
            lastPlayedChapterGuid = productionRecord.LastPlayedChapterGuid;

            stories.Capacity = productionRecord.NumStoryRecords;

            for (int i = 0, n = productionRecord.NumStoryRecords; i < n; ++i)
            {
                stories.Add(new StoryDTO(productionRecord.GetStoryRecord(i)));
            }
        }
    }
}