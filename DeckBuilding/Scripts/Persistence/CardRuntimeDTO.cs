using System;
using System.Collections.Generic;

namespace Celeste.DeckBuilding.Persistence
{
    [Serializable]
    public class CardRuntimeDTO
    {
        public int cardGuid;
        public List<string> componentNames = new List<string>();
        public List<string> componentData = new List<string>();

        public CardRuntimeDTO(CardRuntime cardRuntime)
        {
            cardGuid = cardRuntime.CardGuid;

            for (int i = 0, n = cardRuntime.NumComponents; i < n; ++i)
            {
                var componentHandle = cardRuntime.GetComponent(i);
                componentNames.Add(componentHandle.component.name);
                componentData.Add(componentHandle.instance.data.ToJson());
            }
        }
    }
}