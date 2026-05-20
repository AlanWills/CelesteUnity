using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct EnemyDeckAddsCard : IDeckMatchCommand
    {
        private CardInstance card;

        public EnemyDeckAddsCard(CardInstance card)
        {
            this.card = card;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.EnemyDeckRuntime.AddCardToHand(card);
        }
    }
}