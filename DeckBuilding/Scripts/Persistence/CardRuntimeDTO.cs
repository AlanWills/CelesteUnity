using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Persistence
{
    [Serializable]
    public class CardRuntimeDTO
    {
        public int deckIndex;
        public List<string> componentNames = new List<string>();
        public List<string> componentData = new List<string>();

        public CardRuntimeDTO(CardRuntime cardRuntime)
        {
            deckIndex = cardRuntime.DeckIndex;

            for (int i = 0, n = cardRuntime.NumComponents; i < n; ++i)
            {
                var componentHandle = cardRuntime.GetComponent(i);
                componentNames.Add(componentHandle.component.name);
                componentData.Add(componentHandle.instance.data.ToJson());
            }
        }
    }
}