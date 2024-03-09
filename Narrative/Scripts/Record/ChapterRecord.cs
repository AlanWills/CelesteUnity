using Celeste.DataStructures;
using Celeste.FSM;
using Celeste.FSM.Utils;
using Celeste.Narrative.Characters;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative
{
    public class ChapterRecord : ILinearRuntimeRecord
    {
        #region Properties and Fields

        public StoryRecord StoryRecord { get; }
        public Chapter Chapter { get; }
        public FSMGraphNodePath CurrentNodePath { get; set; } = new FSMGraphNodePath();
        public int CurrentBackgroundGuid { get; set; }
        public float Progress { get; private set; }

        public string ChapterName => Chapter.ChapterName;
        public string ChapterDescription => Chapter.ChapterDescription;
        public Sprite ChapterThumbnail => Chapter.ChapterThumbnail;
        public int NumCharacterRecords => characterRecords.Count;
        public int NumValueRecords => valueRecords.Count;

        private List<CharacterRecord> characterRecords = new List<CharacterRecord>();
        private List<ValueRecord> valueRecords = new List<ValueRecord>();

        #endregion

        public ChapterRecord(
            StoryRecord storyRecord, 
            Chapter chapter, 
            string currentNodePath)
        {
            UnityEngine.Debug.Assert(storyRecord != null, $"Story Record null in ChapterRecord.");
            UnityEngine.Debug.Assert(chapter != null, $"Chapter null in ChapterRecord.");

            StoryRecord = storyRecord;
            Chapter = chapter;
            CurrentNodePath = chapter.HasBakedNarrativeGraph ? new FSMGraphNodePath(chapter.NarrativeGraph, currentNodePath) : FSMGraphNodePath.EMPTY;

            characterRecords.Capacity = chapter.NumCharacters;
            for (int i = 0, n = chapter.NumCharacters; i < n; ++i)
            {
                AddCharacterRecord(chapter.GetCharacter(i), 0);
            }

            for (int i = 0, n = chapter.NumStringValues; i < n; ++i)
            {
                AddValueRecord(chapter.GetStringValue(i));
            }

            for (int i = 0, n = chapter.NumBoolValues; i < n; ++i)
            {
                AddValueRecord(chapter.GetBoolValue(i));
            }

            CalculateProgress();
        }

        public void CalculateProgress()
        {
            NarrativeGraph narrativeGraph = Chapter.NarrativeGraph;
            
            if (narrativeGraph == null)
            {
                UnityEngine.Debug.Log("Unable to calculate progress due to missing narrative graph.");
                return;
            }

            Progress = narrativeGraph.CalculateProgress(CurrentNodePath.Node);
        }

        public void ResetProgress()
        {
            CurrentNodePath = new FSMGraphNodePath();
            Progress = 0;
        }

        public void Complete()
        {
            CurrentNodePath = new FSMGraphNodePath(Chapter.NarrativeGraph.FinishNode);
            Progress = 1;
        }

        #region Character Records

        public CharacterRecord GetCharacterRecord(int index)
        {
            return characterRecords.Get(index);
        }

        public CharacterRecord AddCharacterRecord(
            Character character, 
            int avatarCustomisationGuid)
        {
            CharacterRecord characterRecord = new CharacterRecord(character);
            characterRecord.AvatarCustomisationGuid = avatarCustomisationGuid;
            characterRecords.Add(characterRecord);

            return characterRecord;
        }

        public CharacterRecord FindCharacterRecord(int guid)
        {
            return characterRecords.Find(x => x.Character.Guid == guid);
        }

        public CharacterRecord FindCharacterRecord(Character character)
        {
            return characterRecords.Find(x => x.Character == character);
        }

        #endregion

        #region Value Record

        public ValueRecord GetValueRecord(int index)
        {
            return valueRecords.Get(index);
        }

        public ValueRecord AddValueRecord(StringValue stringValue)
        {
            ValueRecord valueRecord = new ValueRecord(stringValue);
            valueRecords.Add(valueRecord);

            return valueRecord;
        }

        public ValueRecord AddValueRecord(BoolValue boolValue)
        {
            ValueRecord valueRecord = new ValueRecord(boolValue);
            valueRecords.Add(valueRecord);

            return valueRecord;
        }

        public ValueRecord FindValueRecord(string name)
        {
            return valueRecords.Find(x => string.CompareOrdinal(x.Name, name) == 0);
        }

        public void LoadChapterValueRecords()
        {
            for (int i = 0, n = valueRecords.Count; i < n; ++i)
            {
                ValueRecord valueRecord = valueRecords[i];

                switch (valueRecord.Type)
                {
                    case ValueType.String:
                        valueRecord.ApplyTo(Chapter.FindStringValue(valueRecord.Name));
                        break;

                    case ValueType.Bool:
                        valueRecord.ApplyTo(Chapter.FindBoolValue(valueRecord.Name));
                        break;

                    default:
                        UnityEngine.Debug.LogAssertion($"Unhandled value type '{valueRecord.Type}' for value '{valueRecord.Name}'.");
                        break;
                }
            }
        }

        #endregion
    }
}