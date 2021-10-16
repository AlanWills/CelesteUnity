using Celeste.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override void CopyFrom(Choice original)
        {
            base.CopyFrom(original);

            displayText = (original as TextChoice).displayText;
        }
    }
}
