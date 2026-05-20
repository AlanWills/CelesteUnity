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

        public CardRuntimeDTO(CardInstance cardInstance)
        {
            cardGuid = cardInstance.CardGuid;
            deckGuid = cardInstance.DeckGuid;

            for (int i = 0, n = cardInstance.NumComponents; i < n; ++i)
            {
                var componentHandle = cardInstance.GetComponent(i);
                components.Add(ComponentDTO.From(componentHandle));
            }
        }
    }
}