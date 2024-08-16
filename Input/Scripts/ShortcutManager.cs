using Celeste.Parameters.Input;
using System.Collections.Generic;
using UnityEngine;
#if USE_NEW_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Shortcut Manager")]
    public class ShortcutManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private List<Shortcut> shortcuts = new List<Shortcut>();

        #endregion

        #region Unity Methods

        private void Update()
        {
            foreach (Shortcut shortcut in shortcuts)
            {
                if (CheckModifiers(shortcut) && CheckKey(shortcut))
                {
                    shortcut.fired.Invoke();
                }
            }
        }

#endregion

        #region Utility

        private bool CheckModifiers(Shortcut shortcut)
        {
            return CheckModifiers(shortcut.modifiers);
        }

        private bool CheckModifiers(Modifiers modifiers)
        {
            for (int i = 1; i <= (int)(modifiers == Modifiers.None ? ModifiersConstants.EVERYTHING : modifiers); i = i << 1)
            {
                Modifiers currentModifier = (Modifiers)i;

                // It's ok for the modifiers to be down already
                // That means players don't have to press both the modifiers and key at exactly the same time
                // (And generally the modifiers go first)
                bool hasFlag = modifiers.HasFlag(currentModifier);
                bool keyPressed = false;
#if USE_NEW_INPUT_SYSTEM
                keyPressed = Keyboard.current[currentModifier.AsKey()].isPressed;
#endif

                if (hasFlag != keyPressed)
                {
                    // We either have the currentModifier set and it's not down
                    // Or we don't have it set and it is down
                    return false;
                }
            }

            return true;
        }

        private bool CheckKey(Shortcut shortcut)
        {
#if USE_NEW_INPUT_SYSTEM
            return CheckKey(shortcut.key);
#else
            return false;
#endif
        }

#if USE_NEW_INPUT_SYSTEM
        private bool CheckKey(KeyReference keyReference)
        {
            // We should only trigger shortcuts when the primary key is first down
            return Keyboard.current[keyReference.Value].isPressed;
        }
#endif

#endregion
    }
}
