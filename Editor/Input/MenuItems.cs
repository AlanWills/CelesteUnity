using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Celeste.Input
{
    public static class MenuItems
    {
        [MenuItem("Celeste/Input/Create Play Shortcut Profile")]
        public static void CreatePlayShortcutProfileMenuItem()
        {
            UnityEditor.ShortcutManagement.ShortcutManager.instance.CreateProfile("Play");
        }
    }
}
