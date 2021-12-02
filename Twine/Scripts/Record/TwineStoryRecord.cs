namespace Celeste.Twine
{
    public class TwineStoryRecord
    {
        #region Properties and Fields

        public string StoryName { get; }
        public string StoryPath { get; }

        #endregion

        public TwineStoryRecord(string storyName, string storyPath)
        {
            StoryName = storyName;
            StoryPath = storyPath;
        }
    }
}