using Celeste.DataStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.Shuffler
{
    [CreateAssetMenu(fileName = nameof(RNGCardShuffler), menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Shuffler/RNG Card Shuffler", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class RNGCardShuffler : CardShuffler
    {
        public override void Shuffle(List<CardRuntime> cards)
        {
            cards.Shuffle();
        }
    }
}