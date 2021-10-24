using Celeste.FSM;
using Celeste.Narrative;
using Celeste.Narrative.Characters;
using Celeste.Narrative.UI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.Nodes
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

        public DialogueNodeBuilder WithCharacter(Character character)
        {
            dialogueNode.Character = character;
            return this;
        }
    }
}