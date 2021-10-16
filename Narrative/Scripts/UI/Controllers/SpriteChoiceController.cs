using Celeste.Narrative.Choices;
using Celeste.Tools;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Sprite Choice Controller")]
    public class SpriteChoiceController : ChoiceController
    {
        #region Properties and Fields

        [SerializeField] private Image choiceSprite;

        #endregion

        public void Hookup(ISpriteChoice choice, Action<IChoice> onChosen)
        {
            base.Hookup(choice, onChosen);

            choiceSprite.sprite = choice.Sprite;

            // Hook up InvalidBehaviour here
        }
    }
}
