using Celeste.Scene;
using Celeste.Scene.Catalogue;
using Celeste.Tools;
using UnityEditor;

namespace CelesteEditor.Scene
{
    public static class SceneSetUnityMenuItems
    {
        [InitializeOnLoadMethod]
        private static void Init()
        {
            EditorApplication.delayCall += AddForAllSceneSets;
        }

        public static void AddForAllSceneSets()
        {
            foreach (SceneSetCatalogue sceneSetCatalogue in EditorOnly.FindAssets<SceneSetCatalogue>())
            {
                foreach (SceneSet sceneSet in sceneSetCatalogue)
                {
                    AddLoadMenuItem(sceneSet);
                }
            }
        }

        public static void AddLoadMenuItem(SceneSet sceneSet)
        {
            if (string.IsNullOrEmpty(sceneSet.MenuItemPath))
            {
                return;
            }

            var menuExist = Unity.Menu.HasMenuItem(sceneSet.MenuItemPath);

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