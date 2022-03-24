using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;
using Celeste.Twine;
using Celeste.Narrative.Nodes;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = "TryAddCharacterInfo", menuName = "Celeste/Twine/Parser Steps/Try Add Character Info")]
    public class TryAddCharacterInfo : TwineNodeParserStep, IUsesTags, IUsesKeys<Character>
    {
        #region Character Tag Struct

        [Serializable]
        public struct CharacterKey
        {
            public string key;
            public Character character;

            public CharacterKey(string tag, Character character)
            {
                this.key = tag;
                this.character = character;
            }
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private List<CharacterKey> characters = new List<CharacterKey>();

        #endregion

        public bool UsesTag(string tag)
        {
            return characters.Exists(x => string.CompareOrdinal(x.key, tag) == 0);
        }

        public bool UsesKey(string key)
        {
            return characters.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        public void AddKey(string key, Character character)
        {
            characters.Add(new CharacterKey(key, character));
        }

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            return parseContext.TwineNode.Tags.Count > 0;
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            List<string> tags = parseContext.TwineNode.Tags;
            TwineStoryAnalysis analysis = parseContext.Analysis;

            foreach (string tag in tags)
            {
                if (HasCharacter(tag))
                {
                    analysis.foundCharacters.Add(tag);
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

        private Character FindCharacterInTags(IList<string> tags)
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