using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.AI
{
    public static class DeckBuildingAIBlackboardKeys
    {
        public static readonly string CURRENT_RESOURCES = string.Intern("CurrentResources");
        public static readonly string CONTROLLED_DECK = string.Intern("ControlledDeck");
        public static readonly string OPPONENT_DECK = string.Intern("OpponentDeck");
    }
}