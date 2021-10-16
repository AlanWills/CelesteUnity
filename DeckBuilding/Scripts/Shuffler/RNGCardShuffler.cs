using Celeste.DataStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Shuffler
{
    [CreateAssetMenu(fileName = nameof(RNGCardShuffler), menuName = "Celeste/Deck Building/Shuffler/RNG Card Shuffler")]
    public class RNGCardShuffler : CardShuffler
    {
        public override void Shuffle(List<CardRuntime> cards)
        {
            cards.Shuffle();
        }
    }
}