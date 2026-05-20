using Celeste.Debug.Menus;
using Celeste.DeckBuilding.Decks;
using UnityEngine;

namespace Celeste.DeckBuilding.Debug
{
    [CreateAssetMenu(fileName = nameof(DeckDebugMenu), menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Debug/Deck Debug Menu", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class DeckDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private Deck deck;

        #endregion

        protected override void OnDrawMenu()
        {
            for (int i = deck.NumCardsInDrawPile - 1; i >= 0; --i)
            {
                using (var horizontal = new GUILayout.HorizontalScope())
                {
                    CardInstance card = deck.GetCardInDrawPile(i);
                    GUILayout.Label(card.CardName);

                    if (GUILayout.Button("Discard", GUILayout.ExpandWidth(false)))
                    {
                        CardInstance drawPileCard = deck.RemoveCardFromDrawPile(i);
                        deck.AddCardToDiscardPile(drawPileCard);
                    }

                    if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                    {
                        CardInstance drawPileCard = deck.RemoveCardFromDrawPile(i);
                        deck.AddCardToRemovedPile(drawPileCard);
                    }
                }
            }

            for (int i = deck.NumCardsInDiscardPile - 1; i >= 0; --i)
            {
                using (var horizontal = new GUILayout.HorizontalScope())
                {
                    CardInstance card = deck.GetCardInDiscardPile(i);
                    GUILayout.Label(card.CardName);

                    if (GUILayout.Button("Add To Draw", GUILayout.ExpandWidth(false)))
                    {
                        CardInstance discardPileCard = deck.RemoveCardFromDiscardPile(i);
                        deck.AddCardToDrawPile(discardPileCard);
                    }

                    if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                    {
                        CardInstance discardPileCard = deck.RemoveCardFromDrawPile(i);
                        deck.AddCardToRemovedPile(discardPileCard);
                    }
                }
            }

            for (int i = deck.NumCardsInRemovedPile - 1; i >= 0; --i)
            {
                using (var horizontal = new GUILayout.HorizontalScope())
                {
                    CardInstance card = deck.GetCardInRemovedPile(i);
                    GUILayout.Label(card.CardName);

                    if (GUILayout.Button("Add To Draw", GUILayout.ExpandWidth(false)))
                    {
                        CardInstance removedPileCard = deck.RemoveCardFromRemovedPile(i);
                        deck.AddCardToDrawPile(removedPileCard);
                    }

                    if (GUILayout.Button("Discard", GUILayout.ExpandWidth(false)))
                    {
                        CardInstance removedPileCard = deck.RemoveCardFromRemovedPile(i);
                        deck.AddCardToDiscardPile(removedPileCard);
                    }
                }
            }
        }
    }
}
