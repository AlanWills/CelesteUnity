using Celeste.Tools;
using UnityEditor;

namespace CelesteEditor.Tools
{
    public static class CelesteToolsMenuItems
    {
        private const string CLEAR_CACHE_MENU_PATH = "Celeste/Tools/Assets/Clear Cache";
        private const string FIND_MISSING_COMPONENTS_MENU_PATH = "Celeste/Tools/Scenes/Find Missing Components";
        private const string DUPLICATE_MENU_PATH = "Assets/Duplicate";

        [MenuItem(CLEAR_CACHE_MENU_PATH)]
        public static void ClearCacheMenuItem()
        {
            CachingUtility.ClearCache();
        }

        [MenuItem(FIND_MISSING_COMPONENTS_MENU_PATH)]
        public static void FindMissingComponentsInLoadedScenesMenuItem()
        {
            SceneUtility.FindMissingComponentsInLoadedScenes();
        }

        [MenuItem(DUPLICATE_MENU_PATH, true)]
        public static bool ValidateDuplicateMenuItem()
        {
            return Selection.activeObject && AssetDatabase.IsMainAsset(Selection.activeObject);
        }

        [MenuItem(DUPLICATE_MENU_PATH)]
        public static void ExecuteDuplicateMenuItem()
        {
            var selectedObject = Selection.activeObject;
            string assetPath = AssetDatabase.GetAssetPath(selectedObject);
            string newAssetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            if (AssetDatabase.CopyAsset(assetPath, newAssetPath))
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                if (selectedObject is ICustomDuplicateFunctionality)
                {
                    (selectedObject as ICustomDuplicateFunctionality).WhenCreatedAsCopy();
                }
            }
        }
    }
}
