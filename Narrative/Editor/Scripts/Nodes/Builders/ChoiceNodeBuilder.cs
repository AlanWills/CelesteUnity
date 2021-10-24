using Celeste.Narrative;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Choices;
using Celeste.Narrative.UI;
using UnityEngine;

namespace CelesteEditor.Narrative.Nodes
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

        public ChoiceNodeBuilder WithTextChoice(string name, string displayText)
        {
            TextChoice textChoice = choiceNode.AddChoice<TextChoice>(name);
            textChoice.DisplayText = displayText;

            return this;
        }
    }
}