using Celeste.Parameters.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
                if (CheckModifiers(shortcut.modifiers) && CheckKey(shortcut.keyCode))
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
                bool keyDown = UnityEngine.Input.GetKey(currentModifier.AsKeyCode());

                if (hasFlag != keyDown)
                {
                    // We either have the currentModifier set and it's not down
                    // Or we don't have it set and it is down
                    return false;
                }
            }

            return true;
        }

        private bool CheckKey(KeyCodeReference keyCodeReference)
        {
            // We should only trigger shortcuts when the primary key is first down
            return UnityEngine.Input.GetKeyDown(keyCodeReference.Value);
        }

        #endregion
    }
}
