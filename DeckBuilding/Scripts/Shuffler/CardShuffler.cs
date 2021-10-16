using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Shuffler
{
    public abstract class CardShuffler : ScriptableObject
    {
        public abstract void Shuffle(List<CardRuntime> cards);
    }
}