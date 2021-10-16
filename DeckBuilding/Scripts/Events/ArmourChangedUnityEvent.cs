using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct ArmourChangedArgs
    {
        public int oldArmour;
        public int newArmour;

        public ArmourChangedArgs(int oldArmour, int newArmour)
        {
            this.oldArmour = oldArmour;
            this.newArmour = newArmour;
        }
    }

    [Serializable]
    public class ArmourChangedUnityEvent : UnityEvent<ArmourChangedArgs> { }
}