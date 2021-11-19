using Celeste.DataStructures;
using Celeste.Log;
using Celeste.Persistence;
using System;
using System.IO;
using UnityEngine;

namespace Celeste.Twine.Persistence
{
    [AddComponentMenu("Celeste/Twine/Persistence/Twine Persistence")]
    public class TwinePersistence : PersistentSceneSingleton<TwinePersistence, TwineDTO>
    {
        #region Properties and Fields

        public static readonly string FILE_NAME = "Twine.dat";
        protected override string FileName
        {
            get { return FILE_NAME; }
        }

        public int NumTwineStories
        {
            get { return twineDTO != null ? twineDTO.stories.Count : 0; }
        }

        [NonSerialized] private TwineDTO twineDTO;

        #endregion

        public string GetTwineStoryName(int index)
        {
            if (twineDTO == null)
            {
                return "Unknown";
            }

            var twineStoryDTO = twineDTO.stories.Get(index);
            return twineStoryDTO != null ? twineStoryDTO.storyName : "Unknown";
        }

        public TwineStory LoadTwineStory(int index)
        {
            if (twineDTO == null)
            {
                UnityEngine.Debug.LogError("Twine Persistence not loaded yet.");
                return null;
            }

            var twineStoryDTO = twineDTO.stories.Get(index);
            if (twineStoryDTO == null)
            {
                UnityEngine.Debug.LogError($"Could not find Twine Story in Persistence at index {index}.");
                return null;
            }

            string fileContents = File.ReadAllText(Path.Combine(Application.persistentDataPath, twineStoryDTO.storyPath));
            UnityEngine.Debug.Assert(!string.IsNullOrWhiteSpace(fileContents), $"Twine Story at index {index} had an empty save file.");

            TwineStory twineStory = CreateTwineStory(twineStoryDTO.storyName);
            JsonUtility.FromJsonOverwrite(fileContents, twineStory);

            // Initialize after loading our data from json
            twineStory.Initialize();

            return twineStory;
        }

        public TwineStory AddTwineStory(string newName)
        {
            if (twineDTO == null)
            {
                UnityEngine.Debug.LogError("Unable to create story: persistence not loaded.");
                return null;
            }

            if (FindTwineStory(newName) != null)
            {
                UnityEngine.Debug.LogError("Unable to create story: story with name already exists.");
                return null;
            }

            TwineStory twineStory = CreateTwineStory(newName);
            twineStory.Initialize();    // Initialize before adding nodes
            
            TwineNode startNode = twineStory.AddNode("Untitled 1");
            twineStory.startnode = startNode.pid;

            SaveTwineStory(twineStory);

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

            string fileName = Path.GetFileNameWithoutExtension(storyPath);
            SaveTwineStory(fileName, allTextAtPath);
        }

        private TwineStory CreateTwineStory(string storyName)
        {
            TwineStory twineStory = ScriptableObject.CreateInstance<TwineStory>();
            twineStory.name = storyName;

            return twineStory;
        }

        private TwineStoryDTO FindTwineStory(string storyName)
        {
            return twineDTO != null ? twineDTO.stories.Find(x => string.CompareOrdinal(x.storyName, storyName) == 0) : null;
        }

        #region Save/Load Methods

        protected override void Deserialize(TwineDTO dto)
        {
            twineDTO = dto;
        }

        protected override TwineDTO Serialize()
        {
            return twineDTO;
        }

        protected override void SetDefaultValues()
        {
            twineDTO = new TwineDTO();
        }

        private void SaveTwineStory(TwineStory twineStory)
        {
            SaveTwineStory(twineStory.name, JsonUtility.ToJson(twineStory));
        }

        private void SaveTwineStory(string storyName, string serializedStory)
        {
            TwineStoryDTO existingDTO = FindTwineStory(storyName);
            if (existingDTO == null)
            {
                existingDTO = new TwineStoryDTO(storyName, $"{storyName}.{TwineStory.FILE_EXTENSION}");
                twineDTO.stories.Add(existingDTO);
            }

            File.WriteAllText(Path.Combine(Application.persistentDataPath, existingDTO.storyPath), serializedStory);
            HudLog.LogInfo($"Saved Twine Story {storyName} to {existingDTO.storyPath}");

            Save();
        }

        #endregion

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