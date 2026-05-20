using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct CostChangedArgs
    {
        public CardInstance card;
        public int oldCost;
        public int newCost;

        public CostChangedArgs(CardInstance card, int oldCost, int newCost)
        {
            this.card = card;
            this.oldCost = oldCost;
            this.newCost = newCost;
        }
    }

    [Serializable]
    public class CostChangedUnityEvent : UnityEvent<CostChangedArgs> { }
}