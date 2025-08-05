using System.Collections.Generic;
using Celeste.Narrative;
using Celeste.Narrative.Tokens;
using CelesteEditor.FSM.Nodes;

namespace CelesteEditor.Narrative
{
    [CustomNodeEditor(typeof(DialogueNode))]
    public class DialogueNodeEditor : FSMNodeEditor
    {
        #region GUI

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            DialogueNode dialogueNode = target as DialogueNode;
            bool isTextEmptyBefore = string.IsNullOrEmpty(dialogueNode.RawDialogue);
            bool isCharacterSetBefore = dialogueNode.Character != null;
            
            DrawFixGUI();
            dialogueNode.DrawFindDialogueTokensGUI();
            DrawDefaultPortPair();
            DrawDialogueNodeValues();
            serializedObject.ApplyModifiedProperties();
            
            if (isTextEmptyBefore && !string.IsNullOrEmpty(dialogueNode.RawDialogue))
            {
                dialogueNode.FindDialogueTokens();
            }
            else if (!isTextEmptyBefore && string.IsNullOrEmpty(dialogueNode.RawDialogue))
            {
                dialogueNode.DialogueTokens = new List<LocaToken>();
            }

            if (!isCharacterSetBefore && dialogueNode.Character != null)
            {
                dialogueNode.UIPosition = dialogueNode.Character.DefaultUIPosition;
            }
        }

        private void DrawDialogueNodeValues()
        {
            string[] excludes = { "m_Script", "graph", "position", "ports" };
            DrawNodeProperties(serializedObject, excludes);
        }

        #endregion
    }
}
