using Celeste.FSM;
using Celeste.Narrative;
using Celeste.Twine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = nameof(TryCreateNode), menuName = "Celeste/Twine/Parser Steps/Try Create Node")]
    public class TryCreateNode : TwineNodeParserStep
    {
        #region Properties and Fields

        [SerializeField] private List<TwineNodeParserStep> createNodeSteps = new List<TwineNodeParserStep>();

        #endregion

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
    }
}