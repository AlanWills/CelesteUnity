using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateNode), menuName = "Celeste/Twine/Parser Steps/Try Create Node")]
    public class TryCreateNode : TwineNodeParserStep, IUsesTags, IUsesKeys
    {
        #region Properties and Fields

        [SerializeField] private List<TwineNodeParserStep> createNodeSteps = new List<TwineNodeParserStep>();

        #endregion

        public void AddKeyForUse(string key, object obj)
        {
            foreach (TwineNodeParserStep step in createNodeSteps)
            {
                IUsesKeys usesKeys = step as IUsesKeys;
                if (usesKeys != null && usesKeys.CouldUseKey(key, obj))
                {
                    usesKeys.AddKeyForUse(key, obj);
                }
            }
        }

        public bool CouldUseKey(string key, object obj)
        {
            foreach (TwineNodeParserStep step in createNodeSteps)
            {
                if (step is IUsesKeys && (step as IUsesKeys).CouldUseKey(key, obj))
                {
                    return true;
                }
            }

            return false;
        }

        public bool UsesKey(string key)
        {
            foreach (TwineNodeParserStep step in createNodeSteps)
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
            foreach (TwineNodeParserStep step in createNodeSteps)
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
            foreach (TwineNodeParserStep step in createNodeSteps)
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

            // Go through the sub commands and see if one can execute
            for (int i = 0, n = createNodeSteps.Count; i < n; ++i)
            {
                if (createNodeSteps[i].CanParse(parseContext))
                {
                    return true;
                }
            }

            return false;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            for (int i = 0, n = createNodeSteps.Count; i < n; ++i)
            {
                if (createNodeSteps[i].CanParse(parseContext))
                {
                    createNodeSteps[i].Parse(parseContext);
                    return;
                }
            }

            UnityEngine.Debug.LogError($"Failed to create a node for Twine Node {parseContext.TwineNode.Name}.");
        }

        #endregion
    }
}