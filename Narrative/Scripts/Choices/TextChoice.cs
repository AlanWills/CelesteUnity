using System.ComponentModel;
using Celeste.Tools;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative.Choices
{
    [DisplayName("Text Choice")]
    public class TextChoice : Choice, ITextChoice
    {
        #region Properties and Fields

        public string DisplayText
        {
            get { return displayText; }
            set
            {
                displayText = value;
                EditorOnly.SetDirty(this);
            }
        }

        public DialogueType DialogueType
        {
            get { return dialogueType; }
            set
            {
                dialogueType = value;
                EditorOnly.SetDirty(this);
            }
        }

        [SerializeField] private string displayText;
        [SerializeField] private DialogueType dialogueType = DialogueType.Speech;

        #endregion

        public override void OnValidate()
        {
            base.OnValidate();
            
            if (string.IsNullOrEmpty(displayText))
            {
                displayText = ID;
                EditorOnly.SetDirty(this);
            }
        }
    }
}
