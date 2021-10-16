using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Events
{
    [Serializable]
    public struct StrengthChangedArgs
    {
        public int oldStrength;
        public int newStrength;

        public StrengthChangedArgs(int oldStrength, int newStrength)
        {
            this.oldStrength = oldStrength;
            this.newStrength = newStrength;
        }
    }

    [Serializable]
    public class StrengthChangedUnityEvent : UnityEvent<StrengthChangedArgs> { }
}