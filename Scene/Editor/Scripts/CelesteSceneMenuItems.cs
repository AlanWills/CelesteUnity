using Celeste.Scene;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Scene
{
    public static class CelesteSceneMenuItems
    {
        [MenuItem("Celeste/Tools/Update Scenes In Build")]
        public static void UpdateScenesInBuild()
        {
            Dictionary<string, string> scenePathLookup = new Dictionary<string, string>();
            HashSet<string> loadedScenes = new HashSet<string>();

            foreach (string sceneGuid in AssetDatabase.FindAssets($"t:{nameof(SceneAsset)}"))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                SceneAsset unityScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath);
                scenePathLookup[unityScene.name] = assetPath;
            }

            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            Dictionary<string, EditorBuildSettingsScene> newScenes = new Dictionary<string, EditorBuildSettingsScene>();

            foreach (var scene in scenes)
            {
                newScenes[scene.path] = scene;
            }

            foreach (string sceneSetGuid in AssetDatabase.FindAssets($"t:{nameof(SceneSet)}"))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(sceneSetGuid);
                SceneSet sceneSet = AssetDatabase.LoadAssetAtPath<SceneSet>(assetPath);

                for (int i = 0, n = sceneSet.NumScenes; i < n; ++i)
                {
                    string sceneName = sceneSet.GetSceneName(i);
                    string scenePath = scenePathLookup[sceneName];

                    if (!newScenes.ContainsKey(scenePath))
                    {
                        newScenes.Add(scenePath, new EditorBuildSettingsScene(scenePath, true));
                    }
                }
            }

            EditorBuildSettings.scenes = newScenes.Values.ToArray();
            AssetDatabase.SaveAssets();
        }
    }
}