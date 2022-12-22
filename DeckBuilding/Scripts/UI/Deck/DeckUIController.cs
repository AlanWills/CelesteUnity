using Celeste.DeckBuilding.Decks;
using Celeste.Events;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Deck UI Controller")]
    public class DeckUIController : MonoBehaviour
    {
        #region Properties and Fields

        private Transform DrawPileTransform => usesUIComponents? drawPileUI.transform : drawPile.transform;
        private Transform DiscardPileTransform => usesUIComponents ? discardPileUI.transform : discardPile.transform;
        private Transform RemovedPileTransform => usesUIComponents ? removedPileUI.transform : removedPile.transform;

        [SerializeField] private Deck deck;
        [SerializeField] private bool usesUIComponents = true;
        [SerializeField] private bool showDrawPileFaceUp = false;
        [SerializeField] private bool showDiscardPileFaceUp = true;
        [SerializeField] private bool showRemovedPileFaceUp = true;
        [SerializeField] private ShowTooltipEvent showTooltip;
        [SerializeField] private Celeste.Events.Event hideTooltip;

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
                    drawPileUI.sprite = GetDrawPileTopCardSprite();
                }
            }
            else
            {
                drawPile.enabled = !deck.DrawPileEmpty;

                if (!deck.DrawPileEmpty)
                {
                    drawPile.sprite = GetDrawPileTopCardSprite();
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
                    discardPileUI.sprite = GetDiscardPileTopCardSprite();
                }
            }
            else
            {
                discardPile.enabled = !deck.DiscardPileEmpty;

                if (!deck.DiscardPileEmpty)
                {
                    discardPile.sprite = GetDiscardPileTopCardSprite();
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
                    removedPileUI.sprite = GetRemovedPileTopCardSprite();
                }
            }
            else
            {
                removedPile.enabled = !deck.RemovedPileEmpty;

                if (!deck.RemovedPileEmpty)
                {
                    removedPile.sprite = GetRemovedPileTopCardSprite();
                }
            }
        }

        private Sprite GetDrawPileTopCardSprite()
        {
            CardRuntime topCard = deck.PeekTopCardOfDrawPile();
            return showDrawPileFaceUp ? topCard.CardFront : topCard.CardBack;
        }

        private Sprite GetDiscardPileTopCardSprite()
        {
            CardRuntime topCard = deck.PeekTopCardOfDiscardPile();
            return showDiscardPileFaceUp ? topCard.CardFront : topCard.CardBack;
        }

        private Sprite GetRemovedPileTopCardSprite()
        {
            CardRuntime topCard = deck.PeekTopCardOfRemovedPile();
            return showRemovedPileFaceUp ? topCard.CardFront : topCard.CardBack;
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

        public void OnMouseEnterDrawPile(Vector2 mousePosition)
        {
            showTooltip.Invoke(new TooltipArgs()
            {
                position = DrawPileTransform.position,
                isWorldSpace = true,
                text = $"{deck.NumCardsInDrawPile} {deck.DisplayName}"
            });
        }

        public void OnMouseExitDrawPile()
        {
            hideTooltip.Invoke();
        }

        public void OnMouseEnterDiscardPile(Vector2 mousePosition)
        {
            showTooltip.Invoke(new TooltipArgs()
            {
                position = DiscardPileTransform.position,
                isWorldSpace = true,
                text = $"{deck.NumCardsInDiscardPile} Discarded"
            });
        }

        public void OnMouseExitDiscardPile()
        {
            hideTooltip.Invoke();
        }

        public void OnMouseEnterRemovedPile(Vector2 mousePosition)
        {
            showTooltip.Invoke(new TooltipArgs()
            {
                position = RemovedPileTransform.position,
                isWorldSpace = true,
                text = $"{deck.NumCardsInRemovedPile} Removed"
            });
        }

        public void OnMouseExitRemovedPile()
        {
            hideTooltip.Invoke();
        }

        #endregion
    }
}