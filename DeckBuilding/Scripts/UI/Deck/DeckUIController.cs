using Celeste.DeckBuilding.Decks;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Deck UI Controller")]
    public class DeckUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI drawPileCount;
        [SerializeField] private TextMeshProUGUI discardPileCount;
        [SerializeField] private TextMeshProUGUI removedPileCount;

        private Deck deck;

        #endregion

        public void Hookup(Deck deck)
        {
            this.deck = deck;

            UpdateDrawPileUI();
            UpdateDiscardPileUI();
            UpdateRemovedPileUI();
        }

        private void UpdateDrawPileUI()
        {
            drawPileCount.text = deck.NumCardsInDrawPile.ToString();
        }

        private void UpdateDiscardPileUI()
        {
            discardPileCount.text = deck.NumCardsInDiscardPile.ToString();
        }

        private void UpdateRemovedPileUI()
        {
            removedPileCount.text = deck.NumCardsInRemovedPile.ToString();
        }

        #region Callbacks

        public void OnCardAddedToDrawPile(CardRuntime card)
        {
            UpdateDrawPileUI();
        }

        public void OnCardRemovedFromDrawPile(CardRuntime card)
        {
            UpdateDrawPileUI();
        }

        public void OnCardAddedToDiscardPile(CardRuntime card)
        {
            UpdateDiscardPileUI();
        }

        public void OnCardRemovedFromDiscardPile(CardRuntime card)
        {
            UpdateDiscardPileUI();
        }

        public void OnCardAddedToRemovedPile(CardRuntime card)
        {
            UpdateRemovedPileUI();
        }

        public void OnCardRemovedFromRemovedPile(CardRuntime card)
        {
            UpdateRemovedPileUI();
        }

        #endregion
    }
}