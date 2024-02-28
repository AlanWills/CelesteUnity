using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateNode), order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Parser Steps/Try Create Node")]
    public class TryCreateNode : TwineNodeParserStep, IUsesTags, IUsesKeys
    {
        #region Properties and Fields

        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var createNodeStep in createNodeSteps)
                {
                    IUsesKeys usesKeys = createNodeStep as IUsesKeys;
                    
                    if (usesKeys != null)
                    {
                        foreach (string key in usesKeys.Keys)
                        {
                            yield return key;
                        }
                    }
                }
            }
        }

        public IEnumerable<string> Tags
        {
            get
            {
                foreach (var createNodeStep in createNodeSteps)
                {
                    IUsesTags usesTags = createNodeStep as IUsesTags;

                    if (usesTags != null)
                    {
                        foreach (string tag in usesTags.Tags)
                        {
                            yield return tag;
                        }
                    }
                }
            }
        }

        [SerializeField] private List<TwineNodeParserStep> createNodeSteps = new List<TwineNodeParserStep>();

        #endregion

        public void AddKeyForUse(IKey key)
        {
            foreach (TwineNodeParserStep step in createNodeSteps)
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
            foreach (TwineNodeParserStep step in createNodeSteps)
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