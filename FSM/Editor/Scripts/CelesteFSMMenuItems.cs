using Celeste.FSM;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.FSM
{
    public static class CelesteFSMMenuItems
    {
        [MenuItem(CelesteFSMConstants.MAKE_INTO_AN_FSM_RUNTIME_MENU_PATH, true)]
        public static bool ValidateMakeIntoAnFSMRuntimeContextMenuItem()
        {
            return Selection.activeGameObject != null;
        }

        [MenuItem(CelesteFSMConstants.MAKE_INTO_AN_FSM_RUNTIME_MENU_PATH)]
        public static void MakeIntoAnFSMRuntimeContextMenuItem()
        {
            GameObject gameObject = Selection.activeGameObject;
            gameObject.AddComponent<FSMRuntime>();
            EditorUtility.SetDirty(gameObject);
        }
    }
}
