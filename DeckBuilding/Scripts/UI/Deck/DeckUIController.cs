using Celeste.DeckBuilding.Decks;
using Celeste.Tools.Attributes.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Deck UI Controller")]
    public class DeckUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Deck deck;
        [SerializeField] private bool usesUIComponents = true;

        // UI Elements
        [SerializeField, ShowIf(nameof(usesUIComponents))] private Image drawPileUI;
        [SerializeField, ShowIf(nameof(usesUIComponents))] private Image discardPileUI;
        [SerializeField, ShowIf(nameof(usesUIComponents))] private Image removedPileUI;

        // Non UI Elements
        [SerializeField, HideIf(nameof(usesUIComponents))] private SpriteRenderer drawPile;
        [SerializeField, HideIf(nameof(usesUIComponents))] private SpriteRenderer discardPile;
        [SerializeField, HideIf(nameof(usesUIComponents))] private SpriteRenderer removedPile;

        #endregion

        #region Unity Methods

        public void Awake()
        {
            UpdateDrawPileUI();
            UpdateDiscardPileUI();
            UpdateRemovedPileUI();
        }

        #endregion

        private void UpdateDrawPileUI()
        {
            if (usesUIComponents)
            {
                drawPileUI.enabled = !deck.DrawPileEmpty;

                if (!deck.DrawPileEmpty)
                {
                    drawPileUI.sprite = deck.PeekTopCardOfDrawPile().CardBack;
                }
            }
            else
            {
                drawPile.enabled = !deck.DrawPileEmpty;

                if (!deck.DrawPileEmpty)
                {
                    drawPile.sprite = deck.PeekTopCardOfDrawPile().CardBack;
                }
            }
        }

        private void UpdateDiscardPileUI()
        {
            if (usesUIComponents)
            {
                discardPileUI.enabled = !deck.DiscardPileEmpty;

                if (!deck.DiscardPileEmpty)
                {
                    discardPileUI.sprite = deck.PeekTopCardOfDiscardPile().CardFront;
                }
            }
            else
            {
                discardPile.enabled = !deck.DiscardPileEmpty;

                if (!deck.DiscardPileEmpty)
                {
                    discardPile.sprite = deck.PeekTopCardOfDiscardPile().CardFront;
                }
            }
        }

        private void UpdateRemovedPileUI()
        {
            if (usesUIComponents)
            {
                removedPileUI.enabled = !deck.RemovedPileEmpty;

                if (!deck.RemovedPileEmpty)
                {
                    removedPileUI.sprite = deck.PeekTopCardOfRemovedPile().CardFront;
                }
            }
            else
            {
                removedPile.enabled = !deck.RemovedPileEmpty;

                if (!deck.RemovedPileEmpty)
                {
                    removedPile.sprite = deck.PeekTopCardOfRemovedPile().CardFront;
                }
            }
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