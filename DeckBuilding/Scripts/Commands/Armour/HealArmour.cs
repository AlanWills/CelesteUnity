using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct HealArmour : IDeckMatchCommand
    {
        private CardInstance actorToApplyTo;
        private int armourToHeal;

        public HealArmour(CardInstance actorToApplyTo, int armourToHeal)
        {
            this.actorToApplyTo = actorToApplyTo;
            this.armourToHeal = armourToHeal;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            actorToApplyTo.AddArmour(armourToHeal);
        }
    }
}