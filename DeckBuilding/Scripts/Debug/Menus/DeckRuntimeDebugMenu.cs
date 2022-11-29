using Celeste.Debug.Menus;
using Celeste.DeckBuilding.Extensions;
using Celeste.Events;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Debug.Menus
{
    [CreateAssetMenu(fileName = nameof(DeckRuntimeDebugMenu), menuName = "Celeste/Deck Building/Debug/Deck Runtime Debug Menu")]
    public class DeckRuntimeDebugMenu : DebugMenu
    {
        #region Properties and Fields

        private DeckMatchPlayerRuntime deckRuntime;

        #endregion

        public void Hookup(DeckMatchPlayerRuntime deckRuntime)
        {
            this.deckRuntime = deckRuntime;
        }

        protected override void OnDrawMenu()
        {
            if (deckRuntime == null)
            {
                GUILayout.Label("Deck Runtime not hooked up.");
                return;
            }

            if (GUILayout.Button("Lose"))
            {
                deckRuntime.Lose();
            }

            // Available Resources
            {
                GUILayout.Label("Available Resources");
                GUILayout.Space(5);

                if (GUILayout.Button("+"))
                {
                    deckRuntime.AddResources(1);
                }
            }

            // Current Hand
            {
                GUILayout.Label("Current Hand");
                GUILayout.Space(5);

                CurrentHand currentHand = deckRuntime.CurrentHand;
                for (int i = currentHand.NumCards - 1; i >= 0; --i)
                {
                    DrawHandCardGUI(currentHand.GetCard(i));
                }
            }

            // Actors
            {
                GUILayout.Label("Stage");
                GUILayout.Space(5);

                Stage stage = deckRuntime.Stage;
                for (int i = stage.NumCards - 1; i >= 0; --i)
                {
                    DrawActorGUI(stage.GetCard(i));
                }
            }
        }

        private void DrawHandCardGUI(CardRuntime card)
        {
            using (GUILayout.HorizontalScope horizontal = new GUILayout.HorizontalScope())
            {
                GUILayout.Label(card.CardName);

                if (GUILayout.Button("Try Play", GUILayout.ExpandWidth(false)))
                {
                    card.TryPlay();
                }

                if (GUILayout.Button("Flip", GUILayout.ExpandWidth(false)))
                {
                    card.IsFaceUp = !card.IsFaceUp;
                }
            }
        }

        private void DrawActorGUI(CardRuntime card)
        {
            GUILayout.Label(card.CardName);

            using (GUILayout.HorizontalScope horizontal = new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Health");

                if (GUILayout.Button("-", GUILayout.ExpandWidth(false)))
                {
                    card.RemoveHealth(1);
                }

                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                {
                    card.AddHealth(1);
                }
            }
        }
    }
}