using System;
using System.Collections.Generic;

namespace Celeste.Twine.Persistence
{
    [Serializable]
    public class TwineDTO
    {
        public List<TwineStoryDTO> stories = new List<TwineStoryDTO>();
    }
}