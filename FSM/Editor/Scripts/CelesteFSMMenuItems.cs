using Celeste.FSM;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.FSM
{
    public static class CelesteFSMMenuItems
    {
        private const string MAKE_INTO_AN_FSM_RUNTIME_MENU_PATH = "GameObject/Celeste/FSM/Make Into An FSM Runtime";

        [MenuItem(MAKE_INTO_AN_FSM_RUNTIME_MENU_PATH, true)]
        public static bool ValidateMakeIntoAnFSMRuntimeContextMenuItem()
        {
            return Selection.activeGameObject != null;
        }

        [MenuItem(MAKE_INTO_AN_FSM_RUNTIME_MENU_PATH)]
        public static void MakeIntoAnFSMRuntimeContextMenuItem()
        {
            GameObject gameObject = Selection.activeGameObject;
            gameObject.AddComponent<FSMRuntime>();
            EditorUtility.SetDirty(gameObject);
        }
    }
}
