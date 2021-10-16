using Celeste.DeckBuilding.Cards;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Discard Pile UI Controller")]
    public class DiscardPileUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI discardPileCount;

        private DiscardPile discardPile;

        #endregion

        public void Hookup(DiscardPile discardPile)
        {
            this.discardPile = discardPile;

            UpdateUI();
        }

        private void UpdateUI()
        {
            discardPileCount.text = discardPile.NumCards.ToString();
        }

        #region Callbacks

        public void OnCardAdded(CardRuntime card)
        {
            UpdateUI();
        }

        public void OnCleared()
        {
            UpdateUI();
        }

        #endregion
    }
}