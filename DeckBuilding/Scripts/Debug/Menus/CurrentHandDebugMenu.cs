using Celeste.Debug.Menus;
using Celeste.DeckBuilding.Decks;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Debug
{
    [CreateAssetMenu(fileName = nameof(CurrentHandDebugMenu), menuName = "Celeste/Deck Building/Debug/Current Hand Debug Menu")]
    public class CurrentHandDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private CurrentHand currentHand;
        [SerializeField] private List<Deck> sourceDecks = new List<Deck>();

        #endregion

        protected override void OnDrawMenu()
        {
            for (int i = 0, n = sourceDecks.Count; i < n; ++i)
            {
                Deck sourceDeck = sourceDecks[i];

                using (var horizontal = new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(sourceDeck.name);

                    if (GUILayout.Button("Draw From", GUILayout.ExpandWidth(false)))
                    {
                        CardRuntime card = sourceDeck.DrawCard();
                        currentHand.AddCard(card);
                    }
                }
            }

            for (int i = currentHand.NumCards - 1; i >= 0; --i)
            {
                using (var horizontal = new GUILayout.HorizontalScope())
                {
                    CardRuntime card = currentHand.GetCard(i);
                    GUILayout.Label(card.CardName);

                    if (GUILayout.Button("Discard", GUILayout.ExpandWidth(false)))
                    {
                        currentHand.RemoveCard(card);
                    }
                }
            }
        }
    }
}
