using Celeste.Debug.Menus;
using UnityEditor;

namespace CelesteEditor.Debug
{
    [CustomEditor(typeof(DebugMenu), true)]
    public class DebugMenuEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DebugMenu debugMenu = target as DebugMenu;
            debugMenu.DrawMenu();
        }
    }
}
