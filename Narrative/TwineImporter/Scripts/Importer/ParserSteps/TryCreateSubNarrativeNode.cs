using Celeste.FSM.Nodes;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    #region Sub Narrative Struct

    [Serializable]
    public struct SubNarrativeKey : IKey
    {
        string IKey.Key => key;

        public string key;
        public NarrativeGraph subNarrative;

        public SubNarrativeKey(string key, NarrativeGraph subNarrative)
        {
            this.key = key;
            this.subNarrative = subNarrative;
        }
    }

    #endregion

    [CreateAssetMenu(fileName = nameof(TryCreateSubNarrativeNode), order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Parser Steps/Try Create Sub Narrative Node")]
    public class TryCreateSubNarrativeNode : TwineNodeParserStep, IUsesKeys
    {
        #region Properties and Fields

        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var subNarrativeKey in subNarrativeKeys)
                {
                    yield return subNarrativeKey.key;
                }
            }
        }

        [SerializeField] private string instruction = "SubNarrative";
        [SerializeField] private List<SubNarrativeKey> subNarrativeKeys = new List<SubNarrativeKey>();

        #endregion

        public void AddKeyForUse(IKey key)
        {
            subNarrativeKeys.Add((SubNarrativeKey)key);
        }

        public bool CouldUseKey(IKey key)
        {
            return key is SubNarrativeKey;
        }

        public bool UsesKey(IKey key)
        {
            return subNarrativeKeys.Exists(x => string.CompareOrdinal(x.key, key.Key) == 0);
        }

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext parseContext)
        {
            return !string.IsNullOrWhiteSpace(parseContext.StrippedLinksText);
        }

        public override void Analyse(TwineNodeAnalyseContext parseContext)
        {
            FindSubNarratives(parseContext.SplitStrippedLinksText, parseContext.Analysis);
        }

        #endregion

        #region Parse

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            string[] splitText = parseContext.SplitStrippedLinksText;

            if (splitText == null || splitText.Length < 2)
            {
                return false;
            }

            if (!IsInstruction(splitText[0]))
            {
                return false;
            }

            return HasSubNarrative(splitText[1]);
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            string[] splitText = parseContext.SplitStrippedLinksText;

            string subNarrativeKey = splitText[1];
            SubFSMNode subFSMNode = parseContext.Graph.AddNode<SubFSMNode>();
            subFSMNode.SubFSM = FindSubNarrative(subNarrativeKey);
            UnityEngine.Debug.Assert(subFSMNode.SubFSM != null, $"Could not find sub narrative {subNarrativeKey}");

            parseContext.FSMNode = subFSMNode;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(subFSMNode);
            }
#endif
        }

        #endregion

        private bool IsInstruction(string str)
        {
            return string.CompareOrdinal(instruction, str) == 0;
        }

        private bool HasSubNarrative(string key)
        {
            return subNarrativeKeys.Exists(x => string.CompareOrdinal(x.key, key) == 0);
        }

        private NarrativeGraph FindSubNarrative(string key)
        {
            return subNarrativeKeys.Find(x => string.CompareOrdinal(x.key, key) == 0).subNarrative;
        }

        private void FindSubNarratives(string[] splitStrippedLinkText, TwineStoryAnalysis analysis)
        {
            if (splitStrippedLinkText != null &&
                splitStrippedLinkText.Length >= 2 &&
                IsInstruction(splitStrippedLinkText[0]))
            {
                string subNarrativeName = splitStrippedLinkText[1];

                if (HasSubNarrative(subNarrativeName))
                {
                    analysis.AddFoundSubNarrative(subNarrativeName);
                }
                else
                {
                    analysis.AddUnrecognizedKey(subNarrativeName);
                }
            }
        }
    }
}