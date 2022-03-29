using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateBranchNode), menuName = "Celeste/Twine/Parser Steps/Try Create Branch Node")]
    public class TryCreateBranchNode : TwineNodeParserStep, IUsesTags
    {
        #region Properties and Fields

        [SerializeField] private string branchTag = "Branch";
        [SerializeField] private List<TwineNodeParserStep> createBranchNodeSteps = new List<TwineNodeParserStep>();

        #endregion

        public void AddKeyForUse(IKey key)
        {
            foreach (TwineNodeParserStep step in createBranchNodeSteps)
            {
                IUsesKeys usesKeys = step as IUsesKeys;
                if (usesKeys != null && usesKeys.CouldUseKey(key))
                {
                    usesKeys.AddKeyForUse(key);
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(step);
#endif
                }
            }
        }

        public bool CouldUseKey(IKey key)
        {
            foreach (TwineNodeParserStep step in createBranchNodeSteps)
            {
                if (step is IUsesKeys && (step as IUsesKeys).CouldUseKey(key))
                {
                    return true;
                }
            }

            return false;
        }

        public bool UsesKey(IKey key)
        {
            foreach (TwineNodeParserStep step in createBranchNodeSteps)
            {
                if (step is IUsesKeys && (step as IUsesKeys).UsesKey(key))
                {
                    return true;
                }
            }

            return false;
        }

        public bool UsesTag(string tag)
        {
            if (string.CompareOrdinal(tag, branchTag) == 0)
            {
                return true;
            }

            foreach (TwineNodeParserStep step in createBranchNodeSteps)
            {
                if (step is IUsesTags && (step as IUsesTags).UsesTag(tag))
                {
                    return true;
                }
            }

            return false;
        }

        #region Analyse

        public override bool CanAnalyse(TwineNodeAnalyseContext analyseContext)
        {
            return true;
        }

        public override void Analyse(TwineNodeAnalyseContext analyseContext)
        {
            foreach (TwineNodeParserStep step in createBranchNodeSteps)
            {
                if (step.CanAnalyse(analyseContext))
                {
                    step.Analyse(analyseContext);
                }
            }
        }

        #endregion

        #region Parse

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            if (HasInstruction(parseContext.TwineNode.Tags))
            {
                // If we have the branch tag, we parse this as a branch no matter what
                return true;
            }

            // Go through the sub commands and see if one can execute
            for (int i = 0, n = createBranchNodeSteps.Count; i < n; ++i)
            {
                if (createBranchNodeSteps[i].CanParse(parseContext))
                {
                    return true;
                }
            }

            return false;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            // Go through the sub commands and see if one can execute
            for (int i = 0, n = createBranchNodeSteps.Count; i < n; ++i)
            {
                if (createBranchNodeSteps[i].CanParse(parseContext))
                {
                    createBranchNodeSteps[i].Parse(parseContext);
                    return;
                }
            }

            UnityEngine.Debug.LogError($"Failed to create a branch node for Twine Node {parseContext.TwineNode.Name}.");
        }

        #endregion

        private bool HasInstruction(List<string> tags)
        {
            return tags.Contains(branchTag);
        }
    }
}