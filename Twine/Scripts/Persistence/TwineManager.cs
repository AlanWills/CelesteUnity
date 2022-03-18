using Celeste.Assets;
using Celeste.Log;
using Celeste.Persistence;
using Celeste.Twine.Persistence;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Celeste.Twine
{
    [AddComponentMenu("Celeste/Twine/Twine Manager")]
    public class TwineManager : PersistentSceneManager<TwineManager, TwineDTO>, IHasAssets
    {
        #region Properties and Fields

        public static readonly string FILE_NAME = "Twine.dat";
        protected override string FileName
        {
            get { return FILE_NAME; }
        }

        [SerializeField] private TwineRecord twineRecord;

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return true;
        }

        public IEnumerator LoadAssets()
        {
            Load();

            yield break;
        }

        #endregion

        #region Save/Load Methods

        protected override void Deserialize(TwineDTO dto)
        {
            foreach (var twineStory in dto.stories)
            {
                twineRecord.FindOrAddTwineStoryRecord(twineStory.storyName, twineStory.storyPath);
            }
        }

        protected override TwineDTO Serialize()
        {
            return new TwineDTO(twineRecord);
        }

        protected override void SetDefaultValues() { }

        #endregion

        private void SaveTwineStory(TwineStory twineStory)
        {
            SaveTwineStory(twineStory.name, twineStory.ToJson());
        }

        private void SaveTwineStory(string storyName, string serializedStory)
        {
            TwineStoryRecord twineStoryRecord = twineRecord.FindOrAddTwineStoryRecord(storyName);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, twineStoryRecord.StoryPath), serializedStory);
            HudLog.LogInfo($"Saved Twine Story {storyName} to {twineStoryRecord.StoryPath}");
        }

        #region Callbacks

        public void OnTwineStoryLoadedFromDatabase(TwineStory twineStory)
        {
            // We must synchronize our persistence data with the story we've just loaded from the database
            SaveTwineStory(twineStory);

            twineStory.Initialize();
        }

        public void OnSaveTwineStory(TwineStory twineStory)
        {
            SaveTwineStory(twineStory);
        }

        #endregion
    }
}