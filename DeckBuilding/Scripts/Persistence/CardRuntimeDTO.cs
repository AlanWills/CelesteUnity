using Celeste.Components.Persistence;
using System;
using System.Collections.Generic;

namespace Celeste.DeckBuilding.Persistence
{
    [Serializable]
    public class CardRuntimeDTO
    {
        public int cardGuid;
        public int deckGuid;
        public List<ComponentDTO> components = new List<ComponentDTO>();

        public CardRuntimeDTO(CardRuntime cardRuntime)
        {
            cardGuid = cardRuntime.CardGuid;
            deckGuid = cardRuntime.DeckGuid;

            for (int i = 0, n = cardRuntime.NumComponents; i < n; ++i)
            {
                var componentHandle = cardRuntime.GetComponent(i);
                components.Add(ComponentDTO.From(componentHandle));
            }
        }
    }
}