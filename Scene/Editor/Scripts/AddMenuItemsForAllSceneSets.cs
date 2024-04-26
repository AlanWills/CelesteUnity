using Celeste.Scene;
using Celeste.Tools;
using UnityEditor;

namespace CelesteEditor.Scene
{
    public static class AddMenuItemsForAllSceneSets
    {
        [InitializeOnLoadMethod]
        private static void Init()
        {
            EditorApplication.delayCall += VerifyCreateMenu;
        }

        private static void VerifyCreateMenu()
        {
            foreach (SceneSet sceneSet in EditorOnly.FindAssets<SceneSet>())
            {
                if (string.IsNullOrEmpty(sceneSet.MenuItemPath))
                {
                    continue;
                }

                var menuExist = Unity.Menu.MenuItemExists(sceneSet.MenuItemPath);

                if (!menuExist)
                {
                    Unity.Menu.AddMenuItem(sceneSet.MenuItemPath, "", false, 1,
                        () =>
                        {
                            sceneSet.EditorOnly_Load(UnityEngine.SceneManagement.LoadSceneMode.Single);
                        },
                        () => { return true; });
                }
            }
        }
    }
}