using Celeste.Scene;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.EditorGUI;

namespace CelesteEditor.Scene
{
    [CustomEditor(typeof(SceneSet))]
    public class SceneSetEditor : Editor
    {
        private SceneSet mergedSceneSet;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SceneSet sceneSet = target as SceneSet;

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Load", GUILayout.ExpandWidth(false)))
                {
                    sceneSet.EditorOnly_Load(LoadSceneMode.Single);
                }

                if (GUILayout.Button("Add To Build", GUILayout.ExpandWidth(false)))
                {
                    Dictionary<string, string> scenePathLookup = new Dictionary<string, string>();
                    HashSet<string> loadedScenes = new HashSet<string>();

                    foreach (string sceneGuid in AssetDatabase.FindAssets($"t:{typeof(SceneAsset).Name}"))
                    {
                        string assetPath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                        SceneAsset unityScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath);
                        scenePathLookup[unityScene.name] = assetPath;
                    }

                    EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
                    List<EditorBuildSettingsScene> newScenes = new List<EditorBuildSettingsScene>(scenes);

                    SerializedProperty scenesProperty = serializedObject.FindProperty("scenes");
                    for (int i = 0, n = scenesProperty.arraySize; i < n; ++i)
                    {
                        string scenePath = scenePathLookup[scenesProperty.GetArrayElementAtIndex(i).stringValue];
                        if (newScenes.FirstOrDefault(x => string.CompareOrdinal(scenePath, x.path) == 0) == null)
                        {
                            newScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                        }
                    }

                    EditorBuildSettings.scenes = newScenes.ToArray();
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                mergedSceneSet = EditorGUILayout.ObjectField(mergedSceneSet, typeof(SceneSet), false) as SceneSet;

                using (new DisabledScope(mergedSceneSet == null))
                {
                    if (GUILayout.Button("Merge", GUILayout.ExpandWidth(false)))
                    {
                        sceneSet.MergeFrom(mergedSceneSet);
                    }
                }
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
