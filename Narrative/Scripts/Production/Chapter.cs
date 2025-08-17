using Celeste.DataStructures;
using Celeste.FSM;
using Celeste.FSM.Nodes;
using Celeste.Narrative.Characters;
using Celeste.Objects;
using Celeste.Parameters;
using Celeste.Twine;
using System;
using System.Collections.Generic;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = "Chapter", menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Production/Chapter", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class Chapter : ScriptableObject, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set
            {
                if (guid != value)
                {
                    guid = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        public string ChapterName
        {
            get => chapterName;
            set
            {
                if (string.CompareOrdinal(chapterName, value) != 0)
                {
                    chapterName = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        public string ChapterDescription
        {
            get => chapterDescription;
            set
            {
                if (string.CompareOrdinal(chapterDescription, value) != 0)
                {
                    chapterDescription = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        public Sprite ChapterThumbnail => chapterThumbnail;

        public int NumCharacters => characters.Count;
        public int NumStringValues => stringValues.Count;
        public int NumBoolValues => boolValues.Count;
        public int NumIntValues => intValues.Count;

        public bool HasBakedNarrativeGraph => narrativeGraph != null;

        public NarrativeGraph NarrativeGraph
        {
            get 
            { 
                if (narrativeGraph != null)
                {
                    return narrativeGraph;
                }

                if (runtimeCreatedNarrativeGraph != null)
                {
                    return runtimeCreatedNarrativeGraph;
                }

                runtimeCreatedNarrativeGraph = CreateInstance<NarrativeGraph>();
                runtimeCreatedNarrativeGraph.name = $"{name}-Graph (Runtime Created)";

                return narrativeGraph;
            }
        }

        public TwineStory TwineStory
        {
            get => twineStory;
            set
            {
                twineStory = value;
                EditorOnly.SetDirty(this);
            }
        }

        public bool CanEdit => TwineStory != null;

        [SerializeField] private int guid;
        [SerializeField] private string chapterName;
        [SerializeField] private string chapterDescription;
        [SerializeField] private Sprite chapterThumbnail;

        [Header("Narrative Info")]
        [SerializeField] private NarrativeGraph narrativeGraph;
        [SerializeField] private TwineStory twineStory;
        [SerializeField] private List<Character> characters = new();
        [SerializeField] private List<StringValue> stringValues = new();
        [SerializeField] private List<BoolValue> boolValues = new();
        [SerializeField] private List<IntValue> intValues = new();

        [NonSerialized] private NarrativeGraph runtimeCreatedNarrativeGraph;

        #endregion

        #region Characters

        public Character GetCharacter(int index)
        {
            return characters.Get(index);
        }

        public Character FindCharacter(int characterGuid)
        {
            return characters.Find(x => x.Guid == characterGuid);
        }

        #endregion

        #region String Values

        public StringValue GetStringValue(int index)
        {
            return stringValues.Get(index);
        }

        public StringValue FindStringValue(string stringValueName)
        {
            return stringValues.Find(x => string.CompareOrdinal(x.name, stringValueName) == 0);
        }

        #endregion

        #region Bool Values

        public BoolValue GetBoolValue(int index)
        {
            return boolValues.Get(index);
        }

        public BoolValue FindBoolValue(string boolValueName)
        {
            return boolValues.Find(x => string.CompareOrdinal(x.name, boolValueName) == 0);
        }

        #endregion

        #region Bool Values

        public IntValue GetIntValue(int index)
        {
            return intValues.Get(index);
        }

        public IntValue FindIntValue(string intValueName)
        {
            return intValues.Find(x => string.CompareOrdinal(x.name, intValueName) == 0);
        }

        #endregion

        #region Utility Functions

        public void FindCharacters()
        {
            HashSet<Character> charactersLookup = new HashSet<Character>();
            FindCharacters(narrativeGraph, charactersLookup);

            characters.Clear();
            characters.AddRange(charactersLookup);
        }

        private void FindCharacters(FSMGraph narrativeGraph, HashSet<Character> charactersLookup)
        {
            foreach (FSMNode node in narrativeGraph.nodes)
            {
                if (node is ICharacterNode characterNode)
                {
                    if (characterNode.Character != null)
                    {
                        charactersLookup.Add(characterNode.Character);
                    }
                }
                else if (node is SubFSMNode subFsmNode)
                {
                    FindCharacters(subFsmNode.SubFSM, charactersLookup);
                }
            }
        }

        #endregion
    }
}