using Celeste.DataStructures;
using Celeste.FSM;
using Celeste.FSM.Nodes;
using Celeste.Narrative.Characters;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = "Chapter", menuName = "Celeste/Narrative/Production/Chapter")]
    public class Chapter : ScriptableObject
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
        }

        public string ChapterName
        {
            get { return chapterName; }
        }

        public string ChapterDescription
        {
            get { return chapterDescription; }
        }

        public Sprite ChapterThumbnail
        {
            get { return chapterThumbnail; }
        }

        public int NumCharacters
        {
            get { return characters != null ? characters.Length : 0; }
        }

        public int NumStringValues
        {
            get { return stringValues != null ? stringValues.Length : 0; }
        }

        public int NumBoolValues
        {
            get { return boolValues != null ? boolValues.Length : 0; }
        }

        public NarrativeGraph NarrativeGraph
        {
            get { return narrativeGraph; }
        }

        [SerializeField] private int guid;
        [SerializeField] private string chapterName;
        [SerializeField] private string chapterDescription;
        [SerializeField] private Sprite chapterThumbnail;
        [SerializeField] private NarrativeGraph narrativeGraph;
        [SerializeField] private Character[] characters;
        [SerializeField] private StringValue[] stringValues;
        [SerializeField] private BoolValue[] boolValues;

        #endregion

        #region Characters

        public Character GetCharacter(int index)
        {
            return characters.Get(index);
        }

        public Character FindCharacter(int guid)
        {
            return characters.Find(x => x.Guid == guid);
        }

        #endregion

        #region String Values

        public StringValue GetStringValue(int index)
        {
            return stringValues.Get(index);
        }

        public StringValue FindStringValue(string name)
        {
            return stringValues.Find(x => string.CompareOrdinal(x.name, name) == 0);
        }

        #endregion

        #region Bool Values

        public BoolValue GetBoolValue(int index)
        {
            return boolValues.Get(index);
        }

        public BoolValue FindBoolValue(string name)
        {
            return boolValues.Find(x => string.CompareOrdinal(x.name, name) == 0);
        }

        #endregion

        #region Utility Functions

        public void FindCharacters()
        {
            HashSet<Character> charactersLookup = new HashSet<Character>();

            FindCharacters(narrativeGraph, charactersLookup);

            Array.Resize(ref characters, charactersLookup.Count);
            
            int characterIndex = 0;
            foreach (Character character in charactersLookup)
            {
                characters[characterIndex] = character;
                ++characterIndex;
            }
        }

        private void FindCharacters(FSMGraph narrativeGraph, HashSet<Character> charactersLookup)
        {
            foreach (FSMNode node in narrativeGraph.nodes)
            {
                if (node is ICharacterNode)
                {
                    ICharacterNode characterNode = node as ICharacterNode;

                    if (characterNode.Character != null && !charactersLookup.Contains(characterNode.Character))
                    {
                        charactersLookup.Add(characterNode.Character);
                    }
                }
                else if (node is SubFSMNode)
                {
                    SubFSMNode subFSMNode = node as SubFSMNode;

                    FindCharacters(subFSMNode.subFSM, charactersLookup);
                }
            }
        }

        #endregion
    }
}