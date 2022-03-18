using Celeste.DataStructures;
using Celeste.Events;
using Celeste.Log;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Celeste.Twine
{
    [CreateAssetMenu(fileName = nameof(TwineRecord), menuName = "Celeste/Twine/Twine Record")]
    public class TwineRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumTwineStoryRecords
        {
            get { return twineStoryRecords.Count; }
        }

        [SerializeField] private TwineStoryEvent onStoryLoaded;
        [SerializeField] private TwineStoryEvent saveTwineStory;
        [SerializeField] private Events.Event save;

        [NonSerialized] private List<TwineStoryRecord> twineStoryRecords = new List<TwineStoryRecord>();

        #endregion

        public string GetTwineStoryName(int index)
        {
            var twineStoryDTO = twineStoryRecords.Get(index);
            return twineStoryDTO != null ? twineStoryDTO.StoryName : "Unknown";
        }

        public string GetTwineStoryPath(int index)
        {
            var twineStoryDTO = twineStoryRecords.Get(index);
            return twineStoryDTO != null ? twineStoryDTO.StoryPath : string.Empty;
        }

        public TwineStory LoadTwineStory(string storyName)
        {
            TwineStoryRecord twineStoryRecord = FindOrAddTwineStoryRecord(storyName);
            return LoadTwineStory(twineStoryRecord);
        }

        public TwineStory LoadTwineStory(int index)
        {
            TwineStoryRecord twineStoryRecord = twineStoryRecords.Get(index);
            if (twineStoryRecord == null)
            {
                UnityEngine.Debug.LogError($"Could not find Twine Story in record at index {index}.");
                return null;
            }

            return LoadTwineStory(twineStoryRecord);
        }

        private TwineStory LoadTwineStory(TwineStoryRecord twineStoryRecord)
        {
            string fileContents = File.ReadAllText(Path.Combine(Application.persistentDataPath, twineStoryRecord.StoryPath));
            UnityEngine.Debug.Assert(!string.IsNullOrWhiteSpace(fileContents), $"Twine Story {twineStoryRecord.StoryName} had an empty save file.");

            TwineStory twineStory = CreateTwineStoryAsset(twineStoryRecord.StoryName);
            JsonUtility.FromJsonOverwrite(fileContents, twineStory);

            // Initialize after loading our data from json
            twineStory.Initialize();
            onStoryLoaded.Invoke(twineStory);

            return twineStory;
        }

        public TwineStory CreateTwineStory(string newName)
        {
            if (FindTwineStoryRecord(newName) != null)
            {
                UnityEngine.Debug.LogError("Unable to create story: story with name already exists.");
                return null;
            }

            TwineStory twineStory = CreateTwineStoryAsset(newName);
            twineStory.Initialize();    // Initialize before adding nodes
            twineStory.AddNode("Untitled 1");

            saveTwineStory.Invoke(twineStory);

            return twineStory;
        }

        public void ImportTwineStory(string storyPath)
        {
            string allTextAtPath = File.ReadAllText(storyPath);
            
            if (!string.IsNullOrWhiteSpace(allTextAtPath))
            {
                UnityEngine.Debug.LogError($"No text found at path {storyPath}");
                return;
            }

            string storyName = Path.GetFileNameWithoutExtension(storyPath);
            TwineStory twineStory = CreateTwineStoryAsset(storyName);
            JsonUtility.FromJsonOverwrite(allTextAtPath, twineStory);
            UnityEngine.Debug.Assert(twineStory != null, $"Failed to load twine story {storyName}.");

            if (twineStory != null)
            {
                FindOrAddTwineStoryRecord(storyName, storyPath);
                saveTwineStory.Invoke(twineStory);
            }
        }

        private TwineStory CreateTwineStoryAsset(string storyName)
        {
            TwineStory twineStory = ScriptableObject.CreateInstance<TwineStory>();
            twineStory.name = storyName;

            return twineStory;
        }

        private TwineStoryRecord FindTwineStoryRecord(string storyName)
        {
            return twineStoryRecords.Find(x => string.CompareOrdinal(x.StoryName, storyName) == 0);
        }

        public bool HasTwineStoryRecord(string storyName)
        {
            return FindTwineStoryRecord(storyName) != null;
        }

        public TwineStoryRecord FindOrAddTwineStoryRecord(string storyName)
        {
            return FindOrAddTwineStoryRecord(storyName, $"{storyName}.{TwineStory.FILE_EXTENSION}");
        }

        public TwineStoryRecord FindOrAddTwineStoryRecord(string storyName, string storyPath)
        {
            TwineStoryRecord twineStoryRecord = FindTwineStoryRecord(storyName);
            if (twineStoryRecord == null)
            {
                twineStoryRecord = new TwineStoryRecord(storyName, storyPath);
                twineStoryRecords.Add(twineStoryRecord);

                save.Invoke();
            }

            return twineStoryRecord;
        }
    }
}