using System;
using UnityEngine;
using Celeste.Parameters.Input;
#if USE_NEW_INPUT_SYSTEM
using Key = UnityEngine.InputSystem.Key;
#else
using Key = UnityEngine.KeyCode;
#endif

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
        public static Key AsKey(this Modifiers modifier)
        {
            switch (modifier)
            {
                case Modifiers.None:
                    return Key.None;

                case Modifiers.LeftCtrl:
#if USE_NEW_INPUT_SYSTEM
                    return Key.LeftCtrl;
#else
                    return Key.LeftControl;
#endif

                case Modifiers.LeftShift:
                    return Key.LeftShift;

                case Modifiers.LeftAlt:
                    return Key.LeftAlt;

                default:
                    return Key.None;
            }
        }
    }

    public class Shortcut : ScriptableObject
    {
        #region Properties and Fields

        public Modifiers modifiers = default;
        public KeyReference key = default;
        [HideInInspector] public Events.Event fired = default;

        #endregion
    }
}
