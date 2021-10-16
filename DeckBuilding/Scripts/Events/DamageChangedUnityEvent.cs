using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct DamageChangedArgs
    {
        public int oldDamage;
        public int newDamage;

        public DamageChangedArgs(int oldDamage, int newDamage)
        {
            this.oldDamage = oldDamage;
            this.newDamage = newDamage;
        }
    }

    [Serializable]
    public class DamageChangedUnityEvent : UnityEvent<DamageChangedArgs> { }
}