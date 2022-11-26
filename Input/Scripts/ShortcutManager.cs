using Celeste.Parameters.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
                if (CheckModifiers(shortcut.modifiers) && CheckKey(shortcut.key))
                {
                    shortcut.fired.Invoke();
                }
            }
        }

        #endregion

        #region Utility

        private bool CheckModifiers(Modifiers modifiers)
        {
            for (int i = 1; i <= (int)(modifiers == Modifiers.None ? ModifiersConstants.EVERYTHING : modifiers); i = i << 1)
            {
                Modifiers currentModifier = (Modifiers)i;

                // It's ok for the modifiers to be down already
                // That means players don't have to press both the modifiers and key at exactly the same time
                // (And generally the modifiers go first)
                bool hasFlag = modifiers.HasFlag(currentModifier);
                bool keyPressed = Keyboard.current[currentModifier.AsKey()].isPressed;

                if (hasFlag != keyPressed)
                {
                    // We either have the currentModifier set and it's not down
                    // Or we don't have it set and it is down
                    return false;
                }
            }

            return true;
        }

        private bool CheckKey(KeyReference keyReference)
        {
            // We should only trigger shortcuts when the primary key is first down
            return Keyboard.current[keyReference.Value].isPressed;
        }

        #endregion
    }
}
