using Celeste.Narrative.UI;
using UnityEngine;

namespace Celeste.Narrative.Nodes
{
    public struct DialogueNodeBuilder
    {
        #region Properties and Fields

        private IDialogueNode dialogueNode;

        #endregion

        #region Constructor

        private DialogueNodeBuilder(IDialogueNode dialogueNode)
        {
            this.dialogueNode = dialogueNode;
        }

        public static DialogueNodeBuilder WithNode(IDialogueNode dialogueNode)
        {
            return new DialogueNodeBuilder(dialogueNode);
        }

        #endregion

        public DialogueNodeBuilder WithName(string name)
        {
            dialogueNode.name = name;
            return this;
        }

        public DialogueNodeBuilder WithPosition(Vector2 position)
        {
            dialogueNode.Position = position;
            return this;
        }

        public DialogueNodeBuilder WithRawDialogue(string dialogue)
        {
            dialogueNode.RawDialogue = dialogue;
            return this;
        }

        public DialogueNodeBuilder WithUIPosition(UIPosition position)
        {
            dialogueNode.UIPosition = position;
            return this;
        }

        public DialogueNodeBuilder WithDialogueType(DialogueType dialogueType)
        {
            dialogueNode.DialogueType = dialogueType;
            return this;
        }

        public DialogueNodeBuilder WithDialogueTokens(ScriptableObject[] locaTokens)
        {
            dialogueNode.DialogueTokens = locaTokens;
            return this;
        }
    }
}