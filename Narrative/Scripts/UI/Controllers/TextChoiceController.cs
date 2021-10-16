using Celeste.Narrative.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Text Choice Controller")]
    public class TextChoiceController : ChoiceController
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI choiceText;
        [SerializeField] private DialogueTypeStyle[] dialogueTypeStyles;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (dialogueTypeStyles == null || dialogueTypeStyles.Length == 0)
            {
                dialogueTypeStyles = new DialogueTypeStyle[3]
                {
                    new DialogueTypeStyle() { dialogueType = DialogueType.Speech, format = "{0}" },
                    new DialogueTypeStyle() { dialogueType = DialogueType.Thinking, format = "<i>{0}</i>" },
                    new DialogueTypeStyle() { dialogueType = DialogueType.Action, format = "<b>{0}</b>" }
                };
            }
        }

        #endregion

        public void Hookup(ITextChoice choice, Action<IChoice> onChosen)
        {
            base.Hookup(choice, onChosen);

            choiceText.text = choice.DisplayText;

            SetDialogueTypeStyle(choice.DialogueType);
        }

        private void SetDialogueTypeStyle(DialogueType dialogueType)
        {
            for (int i = 0, n = dialogueTypeStyles != null ? dialogueTypeStyles.Length : 0; i < n; ++i)
            {
                DialogueTypeStyle style = dialogueTypeStyles[i];

                if ((style.dialogueType & dialogueType) == dialogueType)
                {
                    choiceText.text = string.Format(style.format, choiceText.text);
                }
            }
        }
    }
}
