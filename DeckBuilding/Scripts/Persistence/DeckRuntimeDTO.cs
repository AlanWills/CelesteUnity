using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Persistence
{
    [Serializable]
    public class DeckRuntimeDTO
    {
        public int availableResources;
        public List<CardRuntimeDTO> cardsInDrawPile = new List<CardRuntimeDTO>();
        public List<CardRuntimeDTO> cardsInHand = new List<CardRuntimeDTO>();
        public List<CardRuntimeDTO> cardsInDiscardPile = new List<CardRuntimeDTO>();
        public List<CardRuntimeDTO> cardsInRemovedPile = new List<CardRuntimeDTO>();
        public List<CardRuntimeDTO> cardsOnStage = new List<CardRuntimeDTO>();

        public DeckRuntimeDTO(DeckRuntime deckRuntime)
        {
            availableResources = deckRuntime.AvailableResources.CurrentResources;

            SerializeCardStorage(deckRuntime.DrawPile, cardsInDrawPile);
            SerializeCardStorage(deckRuntime.CurrentHand, cardsInHand);
            SerializeCardStorage(deckRuntime.DiscardPile, cardsInDiscardPile);
            SerializeCardStorage(deckRuntime.RemovedPile, cardsInRemovedPile);
            SerializeCardStorage(deckRuntime.Stage, cardsOnStage);
        }

        private void SerializeCardStorage(ICardStorage cardStorage, List<CardRuntimeDTO> cardRuntimeDTOs)
        {
            for (int i = 0; i < cardStorage.NumCards; ++i)
            {
                cardRuntimeDTOs.Add(new CardRuntimeDTO(cardStorage.GetCard(i)));
            }
        }
    }
}