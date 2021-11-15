using System;

namespace Celeste.Twine.Persistence
{
    [Serializable]
    public class TwineStoryDTO
    {
        public string storyName;
        public string storyPath;

        public TwineStoryDTO(string name, string path)
        {
            storyName = name;
            storyPath = path;
        }
    }
}