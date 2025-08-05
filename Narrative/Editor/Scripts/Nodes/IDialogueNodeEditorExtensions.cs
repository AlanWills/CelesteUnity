using Celeste.Narrative;
using Celeste.Narrative.Settings;
using Celeste.Narrative.Tokens;
using Celeste.Tools;
using UnityEngine;

namespace CelesteEditor.Narrative
{
    public static class IDialogueNodeEditorExtensions
    {
        public static void DrawFindDialogueTokensGUI(this IDialogueNode dialogueNode)
        {
            LocaTokens globalTokens = NarrativeEditorSettings.GetOrCreateSettings().globalLocaTokens;

            using (new GUIEnabledScope(globalTokens != null))
            {
                if (GUILayout.Button("Find Dialogue Tokens"))
                {
                    FindDialogueTokens(dialogueNode);
                }
            }
        }
        
        public static void FindDialogueTokens(this IDialogueNode dialogueNode)
        {
            LocaTokens globalTokens = NarrativeEditorSettings.GetOrCreateSettings().globalLocaTokens;
            if (globalTokens != null)
            {
                dialogueNode.DialogueTokens = globalTokens.FindLocaTokens(dialogueNode.RawDialogue);
            }
        }
    }
}