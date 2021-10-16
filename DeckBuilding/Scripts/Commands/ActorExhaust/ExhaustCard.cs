using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct ExhaustCard : IDeckMatchCommand
    {
        private CardRuntime card;

        public ExhaustCard(CardRuntime card)
        {
            this.card = card;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            card.Exhaust();
        }
    }
}