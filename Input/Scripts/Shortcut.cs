using Celeste.Parameters.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Input
{
    [Flags]
    public enum Modifiers
    {
        None = 0,
        LeftCtrl = 1,
        LeftShift = 2,
        LeftAlt = 4
    }

    public static class ModifiersConstants
    {
        public const Modifiers EVERYTHING = Modifiers.LeftAlt | Modifiers.LeftShift | Modifiers.LeftCtrl | Modifiers.None;
    }

    public static class ModifiersExtensions
    { 
        public static KeyCode AsKeyCode(this Modifiers modifier)
        {
            switch (modifier)
            {
                case Modifiers.None:
                    return KeyCode.None;

                case Modifiers.LeftCtrl:
                    return KeyCode.LeftControl;

                case Modifiers.LeftShift:
                    return KeyCode.LeftShift;

                case Modifiers.LeftAlt:
                    return KeyCode.LeftAlt;

                default:
                    return KeyCode.None;
            }
        }
    }

    public class Shortcut : ScriptableObject
    {
        #region Properties and Fields

        public Modifiers modifiers = default;
        public KeyCodeReference keyCode = default;
        [HideInInspector] public Events.Event fired = default;

        #endregion
    }
}
