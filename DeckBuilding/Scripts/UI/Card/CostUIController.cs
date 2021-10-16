using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Cost UI Controller")]
    public class CostUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObject costUI;
        [SerializeField] private TextMeshProUGUI costText;

        private CardRuntime card;

        #endregion

        public void Hookup(CardRuntime card)
        {
            this.card = card;

            bool supportsCost = card.SupportsCost();
            costUI.SetActive(supportsCost);

            if (supportsCost)
            {
                var costComponent = card.FindComponent<CostComponent>();
                costComponent.component.AddOnCostChangedCallback(costComponent.instance, OnCostChanged);

                UpdateUI(card.GetCost());
            }
        }

        private void OnDisable()
        {
            if (card.SupportsCost())
            {
                var costComponent = card.FindComponent<CostComponent>();
                costComponent.component.RemoveOnCostChangedCallback(costComponent.instance, OnCostChanged);
            }
        }

        private void UpdateUI(int cardCost)
        {
            costText.text = cardCost.ToString();
        }

        #region Callbacks

        private void OnCostChanged(CostChangedArgs costChangedArgs)
        {
            UpdateUI(costChangedArgs.newCost);
        }

        #endregion
    }
}