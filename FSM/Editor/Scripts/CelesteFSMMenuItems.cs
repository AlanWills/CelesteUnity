using UnityEditor;

namespace CelesteEditor.FSM
{
    public static class CelesteFSMMenuItems
    {
        private const string CREATE_FSM_RUNTIME_MENU_PATH = "GameObject/Celeste/FSM/FSM Runtime";

        [MenuItem(CREATE_FSM_RUNTIME_MENU_PATH, true)]
        public static bool ValidateCreateFSMRuntimeContextMenuItem()
        {
            return true;
        }

        [MenuItem(CREATE_FSM_RUNTIME_MENU_PATH)]
        public static void CreateFSMRuntimeContextMenuItem()
        {
            string assetGuid = Selection.assetGUIDs[0];
            var a = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(assetGuid));
            var o = Selection.activeObject;
            UnityEngine.Debug.Log("Here");
        }
    }
}
