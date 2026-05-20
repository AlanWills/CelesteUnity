using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct ExhaustCard : IDeckMatchCommand
    {
        private CardInstance card;

        public ExhaustCard(CardInstance card)
        {
            this.card = card;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            card.Exhaust();
        }
    }
}