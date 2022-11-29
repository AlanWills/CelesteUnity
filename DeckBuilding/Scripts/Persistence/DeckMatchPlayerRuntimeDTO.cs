using System;
using System.Collections.Generic;

namespace Celeste.DeckBuilding.Persistence
{
    [Serializable]
    public class DeckMatchPlayerRuntimeDTO
    {
        public int availableResources;
        public DeckDTO deck;
        public List<CardRuntimeDTO> cardsInHand = new List<CardRuntimeDTO>();
        public List<CardRuntimeDTO> cardsOnStage = new List<CardRuntimeDTO>();

        public DeckMatchPlayerRuntimeDTO(DeckMatchPlayerRuntime deckRuntime)
        {
            availableResources = deckRuntime.AvailableResources.CurrentResources;
            deck = new DeckDTO(deckRuntime.Deck);

            for (int i = 0; i < deckRuntime.CurrentHand.NumCards; ++i)
            {
                cardsInHand.Add(new CardRuntimeDTO(deckRuntime.CurrentHand.GetCard(i)));
            }

            for (int i = 0; i < deckRuntime.Stage.NumCards; ++i)
            {
                cardsOnStage.Add(new CardRuntimeDTO(deckRuntime.Stage.GetCard(i)));
            }
        }
    }
}