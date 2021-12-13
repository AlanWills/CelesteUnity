using Celeste.DataStructures;
using Celeste.FSM.Utils;
using Celeste.Narrative.Characters;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative
{
    public class ChapterRecord
    {
        #region Properties and Fields

        public StoryRecord StoryRecord { get; }
        public Chapter Chapter { get; }
        public string CurrentNodeGuid { get; set; }
        public string CurrentSubGraphNodeGuid { get; set; }
        public int CurrentBackgroundGuid { get; set; }
        public float Progress { get; private set; }

        public string ChapterName
        {
            get { return Chapter.ChapterName; }
        }

        public string ChapterDescription
        {
            get { return Chapter.ChapterDescription; }
        }

        public Sprite ChapterThumbnail
        {
            get { return Chapter.ChapterThumbnail; }
        }

        public int NumCharacterRecords
        {
            get { return characterRecords.Count; }
        }

        public int NumValueRecords
        {
            get { return valueRecords.Count; }
        }

        private List<CharacterRecord> characterRecords = new List<CharacterRecord>();
        private List<ValueRecord> valueRecords = new List<ValueRecord>();

        #endregion

        public ChapterRecord(
            StoryRecord storyRecord, 
            Chapter chapter, 
            string currentNodeGuid,
            string currentSubGraphNodeGuid)
        {
            UnityEngine.Debug.Assert(storyRecord != null, $"Story Record null in ChapterRecord.");
            UnityEngine.Debug.Assert(chapter != null, $"Chapter null in ChapterRecord.");

            StoryRecord = storyRecord;
            Chapter = chapter;
            CurrentNodeGuid = currentNodeGuid;
            CurrentSubGraphNodeGuid = currentSubGraphNodeGuid;

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
            
            if (narrativeGraph == null || narrativeGraph.finishNode == null)
            {
                UnityEngine.Debug.Log("Unable to calculate progress due to misconfigured narrative graph.");
                return;
            }

            if (string.IsNullOrEmpty(CurrentNodeGuid))
            {
                Progress = 0;
                return;
            }
            else if (string.CompareOrdinal(narrativeGraph.finishNode.Guid, CurrentNodeGuid) == 0)
            {
                Progress = 1;
                return;
            }

            var currentNode = narrativeGraph.FindNode(x => string.CompareOrdinal(x.Guid, CurrentNodeGuid) == 0);
            if (currentNode == null)
            {
                UnityEngine.Debug.LogAssertion($"Could not find node with guid {CurrentNodeGuid}.");
                Progress = 0;
                return;
            }

            // As distance to end gets smaller, the top part -> 0 and 'progress' -> 1
            float progress = 1 - ((float)narrativeGraph.CalculateShortestDistance(currentNode, narrativeGraph.finishNode)
                                  / narrativeGraph.CalculateShortestDistance(narrativeGraph.startNode, narrativeGraph.finishNode));

            // Only finishing at the finish node can be progress of 1 (routes through the graph will vary and we always use the shortest to calculate progress)
            Progress = Mathf.Min(progress, 0.99f);
        }

        public void ResetProgress()
        {
            CurrentNodeGuid = "";
            CurrentSubGraphNodeGuid = "";
            Progress = 0;
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