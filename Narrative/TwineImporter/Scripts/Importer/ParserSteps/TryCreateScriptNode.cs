using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateScriptNode), menuName = "Celeste/Twine/Parser Steps/Try Create Script Node")]
    public class TryCreateScriptNode : TwineNodeParserStep, IUsesTags, IUsesKeys
    {
        #region Properties and Fields

        [SerializeField] private string scriptTag = "Script";
        [SerializeField] private List<TwineNodeParserStep> createScriptNodeSteps = new List<TwineNodeParserStep>();

        #endregion

        public void AddKeyForUse(IKey key)
        {
            foreach (TwineNodeParserStep step in createScriptNodeSteps)
            {
                IUsesKeys usesKeys = step as IUsesKeys;
                if (usesKeys != null && usesKeys.CouldUseKey(key))
                {
                    usesKeys.AddKeyForUse(key);
                }
            }
        }

        public bool CouldUseKey(IKey key)
        {
            foreach (TwineNodeParserStep step in createScriptNodeSteps)
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
            foreach (TwineNodeParserStep step in createScriptNodeSteps)
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
            if (string.CompareOrdinal(tag, scriptTag) == 0)
            {
                return true;
            }

            foreach (TwineNodeParserStep step in createScriptNodeSteps)
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
            foreach (TwineNodeParserStep step in createScriptNodeSteps)
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
                // If we have the script tag, we parse this as a script no matter what
                return true;
            }

            // Go through the sub commands and see if one can execute
            for (int i = 0, n = createScriptNodeSteps.Count; i < n; ++i)
            {
                if (createScriptNodeSteps[i].CanParse(parseContext))
                {
                    return true;
                }
            }

            return false;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            // Go through the sub commands and see if one can execute
            for (int i = 0, n = createScriptNodeSteps.Count; i < n; ++i)
            {
                if (createScriptNodeSteps[i].CanParse(parseContext))
                {
                    createScriptNodeSteps[i].Parse(parseContext);
                    return;
                }
            }

            UnityEngine.Debug.LogError($"Failed to create a script node for Twine Node {parseContext.TwineNode.Name}.");
        }

        #endregion

        private bool HasInstruction(List<string> tags)
        {
            return tags.Contains(scriptTag);
        }
    }
}