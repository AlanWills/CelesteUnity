using System;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/UInt Event Raiser")]
    public class UIntEventRaiser : ParameterisedEventRaiser<uint, UIntEvent>
    {
        /// <summary>
        ///  This has to exit because (for some reason) uint functions are not serialized for buttons.
        ///  It's bloody stupid I know, but it will validate for negative values.
        /// </summary>
        /// <param name="argument"></param>
        public void Raise(int argument)
        {
            Raise((uint)Math.Max(0, argument));
        }
    }
}
