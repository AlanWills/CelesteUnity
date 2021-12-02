using System;
using System.Collections.Generic;

namespace Celeste.Twine.Persistence
{
    [Serializable]
    public class TwineDTO
    {
        public List<TwineStoryDTO> stories = new List<TwineStoryDTO>();

        public TwineDTO(TwineRecord twineRecord)
        {
            for (int i = 0, n = twineRecord.NumTwineStoryRecords; i < n; ++i)
            {
                string twineStoryName = twineRecord.GetTwineStoryName(i);
                string twineStoryPath = twineRecord.GetTwineStoryPath(i);
                stories.Add(new TwineStoryDTO(twineStoryName, twineStoryPath));
            }
        }
    }
}