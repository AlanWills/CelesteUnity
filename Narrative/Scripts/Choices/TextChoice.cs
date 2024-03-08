using System.ComponentModel;
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
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public DialogueType DialogueType
        {
            get { return dialogueType; }
            set
            {
                dialogueType = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        [SerializeField] private string displayText;
        [SerializeField] private DialogueType dialogueType = DialogueType.Speech;

        #endregion

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(displayText))
            {
                displayText = ID;
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }
    }
}
