using System;

namespace CelesteEditor.Unity
{
    public static class Menu
    {
        public static void RemoveMenuItem(string name) => UnityEditor.Menu.RemoveMenuItem(name);
        public static bool HasMenuItem(string menuPath) => UnityEditor.Menu.MenuItemExists(menuPath);

        public static void AddMenuItem(string name, string shortcut, bool @checked, int priority,
            Action execute, Func<bool> validate)
        {
            UnityEditor.Menu.AddMenuItem(name, shortcut, @checked, priority, execute, validate);
        }

        public static void RebuildAllMenuItems() => UnityEditor.Menu.RebuildAllMenus();
    }
}