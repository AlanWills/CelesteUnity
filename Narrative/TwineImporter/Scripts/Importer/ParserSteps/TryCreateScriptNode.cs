using Celeste.Twine;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateScriptNode), menuName = "Celeste/Twine/Parser Steps/Try Create Script Node")]
    public class TryCreateScriptNode : TwineNodeParserStep
    {
        #region Properties and Fields

        [SerializeField] private List<TwineNodeParserStep> createScriptNodeSteps = new List<TwineNodeParserStep>();

        #endregion

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            if (parseContext.FSMNode != null)
            {
                return false;
            }

            TwineNode twineNode = parseContext.TwineNode;
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;

            if (importerSettings.ContainsScriptTag(twineNode.Tags))
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
    }
}