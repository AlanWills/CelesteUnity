using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.Persistence
{
    [Serializable]
    public class ProductionDTO
    {
        public int lastPlayedStoryGuid;
        public int lastPlayedChapterGuid;
        public List<StoryDTO> stories = new List<StoryDTO>();

        public ProductionDTO() { }

        public ProductionDTO(ProductionRecord productionRecord)
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