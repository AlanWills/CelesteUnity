using Celeste.Logic;
using Celeste.Narrative;
using Celeste.Narrative.Choices;
using UnityEngine;

namespace Celeste.Narrative.Nodes
{
    public struct ChoiceNodeBuilder
    {
        #region Properties and Fields

        private IChoiceNode choiceNode;

        #endregion

        #region Constructor

        private ChoiceNodeBuilder(IChoiceNode choiceNode)
        {
            this.choiceNode = choiceNode;
        }

        public static ChoiceNodeBuilder WithNode(IChoiceNode choiceNode)
        {
            return new ChoiceNodeBuilder(choiceNode);
        }

        #endregion

        public ChoiceNodeBuilder WithTextChoice(
            string name, 
            string displayText,
            Object[] locaTokens,
            Condition[] conditions)
        {
            TextChoice textChoice = choiceNode.AddChoice<TextChoice>(name);
            textChoice.DisplayText = displayText;
            //textChoice.DisplayTokens = locaTokens;
            textChoice.Conditions = conditions;

            return this;
        }
    }
}