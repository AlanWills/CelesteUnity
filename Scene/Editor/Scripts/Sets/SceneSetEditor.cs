using Celeste.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CelesteEditor.Scene
{
    [CustomEditor(typeof(SceneSet))]
    public class SceneSetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Load", GUILayout.ExpandWidth(false)))
            {
                (target as SceneSet).EditorOnly_Load(LoadSceneMode.Single);
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

            EditorGUILayout.EndHorizontal();

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
