using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct HealthChangedArgs
    {
        public int oldHealth;
        public int newHealth;

        public HealthChangedArgs(int oldHealth, int newHealth)
        {
            this.oldHealth = oldHealth;
            this.newHealth = newHealth;
        }
    }

    [Serializable]
    public class HealthChangedUnityEvent : UnityEvent<HealthChangedArgs> { }
}