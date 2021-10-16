using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Strength UI Controller")]
    public class StrengthUIController
        : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObject strengthUI;
        [SerializeField] private TextMeshProUGUI strengthText;

        private CardRuntime card;

        #endregion

        public void Hookup(CardRuntime card)
        {
            this.card = card;

            bool supportsCombat = card.SupportsCombat();
            strengthUI.SetActive(supportsCombat);

            if (supportsCombat)
            {
                card.AddOnStrengthChangedCallback(OnStrengthChanged);
                UpdateUI(card.GetStrength());
            }
        }

        private void OnDisable()
        {
            if (card.SupportsCombat())
            {
                card.RemoveOnStrengthChangedCallback(OnStrengthChanged);
            }
        }

        private void UpdateUI(int strength)
        {
            strengthText.text = strength.ToString();
        }

        #region Callbacks

        private void OnStrengthChanged(StrengthChangedArgs strengthChangedArgs)
        {
            UpdateUI(strengthChangedArgs.newStrength);
        }

        #endregion
    }
}