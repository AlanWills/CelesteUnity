using Celeste.Scene;
using CelesteEditor.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace CelesteEditor.Scene
{
    public static class CelesteSceneMenuItems
    {
        [MenuItem("Celeste/Tools/Scenes/Update Scenes In Build")]
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
                    string sceneId = sceneSet.GetSceneId(i);
                    SceneType sceneType = sceneSet.GetSceneType(i);

                    if (sceneType == SceneType.Baked)
                    {
                        string scenePath = scenePathLookup[sceneId];

                        if (!newScenes.ContainsKey(scenePath))
                        {
                            newScenes.Add(scenePath, new EditorBuildSettingsScene(scenePath, true));
                        }
                    }
                    else if (sceneType == SceneType.Addressable)
                    {
                        // Also handle the case where the address has the file path at the end
                        // We don't want to support that at runtime, but we do want to find it so we can fix it
                        if (scenePathLookup.TryGetValue(sceneId, out string scenePath) ||
                            scenePathLookup.TryGetValue($"{sceneId}.unity", out scenePath))
                        {
                            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                            sceneAsset.SetAddressableAddress(sceneId);

                            if (newScenes.ContainsKey(scenePath))
                            {
                                // Remove an existing baked in scene because it is now addressable
                                newScenes.Remove(scenePath);
                            }
                        }
                        else
                        {
                            UnityEngine.Debug.LogAssertion($"Failed to find scene path for {sceneId}.");
                        }
                    }
                }
            }

            EditorBuildSettings.scenes = newScenes.Values.ToArray();
            AssetDatabase.SaveAssets();
        }
    }
}