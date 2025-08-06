using System.Collections.Generic;
using Celeste.Narrative;
using Celeste.Narrative.Tokens;
using CelesteEditor.FSM.Nodes;

namespace CelesteEditor.Narrative
{
    [CustomNodeEditor(typeof(NarratorNode))]
    public class NarratorNodeEditor : FSMNodeEditor
    {
        #region GUI

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            NarratorNode narratorNode = target as NarratorNode;
            bool isTextEmptyBefore = string.IsNullOrEmpty(narratorNode.RawDialogue);
            
            DrawFixGUI();
            narratorNode.DrawFindDialogueTokensGUI();
            DrawDefaultPortPair();
            DrawDialogueNodeValues();
            serializedObject.ApplyModifiedProperties();
            
            if (isTextEmptyBefore && !string.IsNullOrEmpty(narratorNode.RawDialogue))
            {
                narratorNode.FindDialogueTokens();
            }
            else if (!isTextEmptyBefore && string.IsNullOrEmpty(narratorNode.RawDialogue))
            {
                narratorNode.DialogueTokens = new List<LocaToken>();
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
