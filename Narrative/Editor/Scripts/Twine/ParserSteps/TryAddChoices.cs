using Celeste.Logic;
using Celeste.Narrative;
using CelesteEditor.Narrative.Nodes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CelesteEditor.Narrative.Twine.ParserSteps
{
    [CreateAssetMenu(fileName = "TryAddChoices", menuName = "Celeste/Narrative/Twine/Parser Steps/Try Add Choices")]
    public class TryAddChoices : TwineNodeParserStep
    {
        #region Properties and Fields

        [NonSerialized] private List<Condition> conditions = new List<Condition>();
        [NonSerialized] private List<ScriptableObject> locaTokens = new List<ScriptableObject>();

        #endregion

        public override bool CanParse(TwineNodeParseContext parseContext)
        {
            return parseContext.FSMNode is IChoiceNode;
        }

        public override void Parse(TwineNodeParseContext parseContext)
        {
            TwineNode node = parseContext.TwineNode;
            IChoiceNode choiceNode = parseContext.FSMNode as IChoiceNode;
            TwineStoryImporterSettings importerSettings = parseContext.ImporterSettings;
            ChoiceNodeBuilder choiceNodeBuilder = ChoiceNodeBuilder.WithNode(choiceNode);

            foreach (TwineNodeLink link in node.links)
            {
                string choiceDisplayText = importerSettings.ReplaceConditions(link.name, conditions);
                choiceDisplayText = importerSettings.ReplaceLocaTokens(choiceDisplayText, locaTokens);

                choiceNodeBuilder.WithTextChoice(
                    link.link,
                    choiceDisplayText,
                    locaTokens.ToArray(),
                    conditions.ToArray());
            }
        }
    }
}