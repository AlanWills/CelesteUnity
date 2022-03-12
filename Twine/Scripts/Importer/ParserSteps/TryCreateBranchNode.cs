using Celeste.Twine;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateBranchNode), menuName = "Celeste/Twine/Parser Steps/Try Create Branch Node")]
    public class TryCreateBranchNode : TwineNodeParserStep
    {
        #region Properties and Fields

        [SerializeField] private List<TwineNodeParserStep> createBranchNodeSteps = new List<TwineNodeParserStep>();

        #endregion

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;

            if (importerSettings.ContainsBranchTag(twineNode.Tags))
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
    }
}