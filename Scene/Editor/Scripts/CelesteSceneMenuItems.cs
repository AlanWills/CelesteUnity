using Celeste.Scene;
using CelesteEditor.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace CelesteEditor.Scene
{
    public static class CelesteSceneMenuItems
    {
        [MenuItem("Celeste/Tools/Update Scenes In Build")]
        public static void UpdateScenesInBuild()
        {
            HashSet<string> loadedScenes = new HashSet<string>();
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            Dictionary<string, EditorBuildSettingsScene> newScenes = new Dictionary<string, EditorBuildSettingsScene>();
            Dictionary<string, string> scenePathLookup = SceneUtility.CreateScenePathLookup();

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
                    SceneSetEntry sceneEntry = sceneSet.GetSceneEntry(i);

                    if (sceneEntry.sceneType == SceneType.Baked)
                    {
                        string scenePath = scenePathLookup[sceneEntry.sceneId];

                        if (!newScenes.ContainsKey(scenePath))
                        {
                            newScenes.Add(scenePath, new EditorBuildSettingsScene(scenePath, true));
                        }
                    }
                    else if (sceneEntry.sceneType == SceneType.Addressable)
                    {
                        if (scenePathLookup.TryGetValue(sceneEntry.sceneId, out string scenePath))
                        {
                            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                            sceneAsset.SetAddressableAddress(sceneEntry.sceneId);

                            if (newScenes.ContainsKey(scenePath))
                            {
                                // Remove an existing baked in scene because it is now addressable
                                newScenes.Remove(scenePath);
                            }
                        }
                        else
                        {
                            UnityEngine.Debug.LogAssertion($"Failed to find scene path for {sceneEntry.sceneId}.");
                        }
                    }
                }
            }

            EditorBuildSettings.scenes = newScenes.Values.ToArray();
            AssetDatabase.SaveAssets();
        }
    }
}