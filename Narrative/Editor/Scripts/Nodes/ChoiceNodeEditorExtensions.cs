using Celeste.Narrative;
using Celeste.Narrative.Settings;
using Celeste.Narrative.Tokens;
using Celeste.Tools;
using UnityEngine;

namespace CelesteEditor.Narrative
{
    public static class ChoiceNodeEditorExtensions
    {
        public static void DrawFindDialogueTokensGUI(this ChoiceNode choiceNode)
        {
            LocaTokens globalTokens = NarrativeEditorSettings.GetOrCreateSettings().globalLocaTokens;

            using (new GUIEnabledScope(globalTokens != null))
            {
                if (GUILayout.Button("Find Dialogue Tokens"))
                {
                    FindDialogueTokens(choiceNode);
                }
            }
        }
        
        public static void FindDialogueTokens(this ChoiceNode choiceNode)
        {
            LocaTokens globalTokens = NarrativeEditorSettings.GetOrCreateSettings().globalLocaTokens;
            if (globalTokens != null)
            {
                var foundDialogueTokens = globalTokens.FindLocaTokens(choiceNode.RawDialogue);

                foreach (var choice in choiceNode.Choices)
                {
                    if (choice is ITextChoice textChoice)
                    {
                        var choiceTokens = globalTokens.FindLocaTokens(textChoice.DisplayText);
                        foundDialogueTokens.AddRange(choiceTokens);
                    }
                }
                
                choiceNode.DialogueTokens = foundDialogueTokens;
            }
        }
    }
}