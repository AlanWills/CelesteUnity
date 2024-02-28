using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;
using Celeste.Twine;
using Celeste.Narrative.Nodes;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    #region Character Tag Struct

    [Serializable]
    public struct CharacterKey : IKey
    {
        string IKey.Key => key;

        public string key;
        public Character character;

        public CharacterKey(string key, Character character)
        {
            this.key = key;
            this.character = character;
        }
    }

    #endregion

    [CreateAssetMenu(fileName = "TryAddCharacterInfo", order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Parser Steps/Try Add Character Info")]
    public class TryAddCharacterInfo : TwineNodeParserStep, IUsesTags, IUsesKeys
    {
        #region Properties and Fields

        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var character in characters)
                {
                    yield return character.key;
                }
            }
        }

        public IEnumerable<string> Tags
        {
            get
            {
                foreach (var character in characters)
                {
                    yield return character.key;
                }
            }
        }

        [SerializeField] private List<CharacterKey> characters = new List<CharacterKey>();

        #endregion

        public bool UsesTag(string tag)
        {
            return characters.Exists(x => string.CompareOrdinal(x.key, tag) == 0);
        }

        public bool UsesKey(IKey key)
        {
            return characters.Exists(x => string.CompareOrdinal(x.key, key.Key) == 0);
        }

        public bool CouldUseKey(IKey key)
        {
            return key is CharacterKey;
        }

        public void AddKeyForUse(IKey key)
        {
            characters.Add((CharacterKey)key);
        }

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            return parseContext.TwineNode.Tags.Count > 0;
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            IReadOnlyList<string> tags = parseContext.TwineNode.Tags;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            foreach (string tag in tags)
            {
                if (HasCharacter(tag))
                {
                    analysis.AddFoundCharacter(tag);
                }
            }
        }

        #endregion

        #region Parse

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return parseContext.FSMNode is ICharacterNode;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineNode node = parseContext.TwineNode;
            ICharacterNode characterNode = parseContext.FSMNode as ICharacterNode;

            Character character = FindCharacterInTags(node.Tags);
            UnityEngine.Debug.Assert(character != null, $"Could not find character for node {node.Name} ({node.pid}).");
            UIPosition characterDefaultPosition = character != null ? character.DefaultUIPosition : UIPosition.Centre;

            CharacterNodeBuilder.
                WithNode(characterNode).
                WithUIPosition(characterDefaultPosition).
                WithCharacter(character);
        }

        #endregion

        private bool HasCharacter(string tag)
        {
            return characters.Exists(x => string.CompareOrdinal(x.key, tag) == 0);
        }

        private Character FindCharacterInTags(IReadOnlyList<string> tags)
        {
            for (int i = 0, n = tags != null ? tags.Count : 0; i < n; ++i)
            {
                var characterTagIndex = characters.FindIndex(x => string.CompareOrdinal(x.key, tags[i]) == 0);
                if (characterTagIndex != -1)
                {
                    return characters[characterTagIndex].character;
                }
            }

            return null;
        }
    }
}