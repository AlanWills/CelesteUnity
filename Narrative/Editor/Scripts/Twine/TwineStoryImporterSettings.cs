using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;
using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using CelesteEditor.Narrative.Twine.ParserSteps;
using Celeste.FSM;

namespace CelesteEditor.Narrative.Twine
{
    [CreateAssetMenu(fileName = nameof(TwineStoryImporterSettings), menuName = "Celeste/Narrative/Twine/Twine Story Importer Settings")]
    public class TwineStoryImporterSettings : ScriptableObject
    {
        #region UIPosition Tag Struct

        [Serializable]
        public struct UIPositionTag
        {
            public string tag;
            public UIPosition position;
        }

        #endregion

        #region Character Tag Struct

        [Serializable]
        public struct CharacterTag
        {
            public string tag;
            public Character character;

            public CharacterTag(string tag, Character character)
            {
                this.tag = tag;
                this.character = character;
            }
        }

        #endregion

        #region Loca Token Tag Struct

        [Serializable]
        public struct LocaTokenTag
        {
            public string tag;
            public ScriptableObject token;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private TwineNodeParserStep[] parserSteps;

        [Header("General Tags")]
        [SerializeField] private string ignoreTag = "Ignore";
        [SerializeField] private string thinkingTag = "Thinking";
        [SerializeField] private string finishTag = "Finish";
        [SerializeField] private UIPositionTag[] uiPositionTags;
        public List<CharacterTag> characterTags = new List<CharacterTag>();
        public List<LocaTokenTag> locaTokenTags = new List<LocaTokenTag>();

        [Header("Graph Settings")]
        public Vector2 nodeSpread = new Vector2(2, 2);

        [Header("File System Settings")]
        public string charactersDirectory;

        #endregion

        #region Parsing

        public bool TryParse(
            TwineNode twineNode,
            FSMGraph graph,
            Vector2 startingNodePosition,
            out FSMNode output)
        {
            bool parsingSuccessful = false;
            
            TwineNodeParseContext parseContext = new TwineNodeParseContext()
            {
                TwineNode = twineNode,
                Graph = graph,
                ImporterSettings = this,
                StartingNodePosition = startingNodePosition
            };

            for (int i = 0, n = parserSteps.Length; i < n && !parseContext.StopParsing; ++i)
            {
                if (parserSteps[i].CanParse(parseContext))
                {
                    parserSteps[i].Parse(parseContext);
                    parsingSuccessful = true;
                }
            }
            
            output = parseContext.FSMNode;
            return parsingSuccessful;
        }

        #endregion

        #region Tag Utility

        public Character FindCharacterFromTag(string[] tags)
        {
            for (int i = 0, n = tags != null ? tags.Length : 0; i < n; ++i)
            {
                var characterTagIndex = characterTags.FindIndex(x => string.CompareOrdinal(x.tag, tags[i]) == 0);
                if (characterTagIndex != -1)
                {
                    return characterTags[characterTagIndex].character;
                }
            }

            return null;
        }

        public UIPosition FindUIPositionFromTag(string[] tags, UIPosition fallbackValue)
        {
            for (int i = 0, n = tags != null ? tags.Length : 0; i < n; ++i)
            {
                var positionTagIndex = uiPositionTags.FindIndex(x => string.CompareOrdinal(x.tag, tags[i]) == 0);
                if (positionTagIndex != -1)
                {
                    return uiPositionTags[positionTagIndex].position;
                }
            }

            return fallbackValue;
        }

        public bool ContainsIgnoreTag(string[] tags)
        {
            return ContainsTag(tags, ignoreTag);
        }

        public bool ContainsThinkingTag(string[] tags)
        {
            return ContainsTag(tags, thinkingTag);
        }

        private bool ContainsTag(string[] tags, string desiredTag)
        {
            return Array.Exists(tags, (string s) => string.CompareOrdinal(s, desiredTag) == 0);
        }

        public bool CouldBeUnregisteredCharacterTag(string tag)
        {
            if (string.CompareOrdinal(tag, thinkingTag) == 0)
            {
                return false;
            }

            if (string.CompareOrdinal(tag, ignoreTag) == 0)
            {
                return false;
            }

            if (string.CompareOrdinal(tag, finishTag) == 0)
            {
                return false;
            }

            return !IsCharacterTagRegistered(tag);
        }

        public bool IsCharacterTagRegistered(string tag)
        {
            return characterTags.Exists(x => string.CompareOrdinal(x.tag, tag) == 0);
        }

        #endregion
    }
}