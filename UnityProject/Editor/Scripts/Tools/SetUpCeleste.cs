using Celeste.Scene;
using Celeste.Tools.Attributes.GUI;
using CelesteEditor.BuildSystem;
using CelesteEditor.Tools;
using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CelesteEditor.UnityProject
{
    [Serializable]
    public struct SetUpCelesteParameters
    {
        #region Properties and Fields

        [Header("Build System")]
        public bool needsBuildSystem;
        [ShowIf(nameof(needsBuildSystem))] public bool runsOnWindows;
        [ShowIf(nameof(needsBuildSystem))] public bool runsOnAndroid;
        [ShowIf(nameof(needsBuildSystem))] public bool runsOniOS;
        [ShowIf(nameof(needsBuildSystem))] public bool runsOnWebGL;

        [Header("Code")]
        public string rootNamespaceName;

        [Header("Assets")]
        public bool usesAddressables;
        public bool usesTextMeshPro;

        [Header("Scenes")]
        public bool needsStartupScene;
        public bool needsBootstrapScene;
        public bool needsEngineSystemsScene;

        #endregion

        public void UseDefaults()
        {
            needsBuildSystem = true;
            runsOnWindows = true;
            runsOnAndroid = true;
            runsOniOS = true;
            runsOnWebGL = true;

            usesAddressables = true;
            usesTextMeshPro = true;

            needsStartupScene = true;
            needsBootstrapScene = true;
            needsEngineSystemsScene = true;
        }
    }

    public static class SetUpCeleste
    {
        #region Properties and Fields

        private const string IMPORT_TMPRO_ESSENTIALS_MENU_ITEM = "Window/TextMeshPro/Import TMP Essential Resources";

        #endregion

        public static void Execute(SetUpCelesteParameters parameters)
        {
            CreateAssetData(parameters);
            CreateBuildSystemData(parameters);
            CreateSceneData(parameters);
        }

        #region Utility

        private static void CreateBuildSystemData(SetUpCelesteParameters parameters)
        {
            if (!parameters.needsBuildSystem)
            {
                return;
            }

            if (parameters.runsOnWindows)
            {
                AllPlatformSettings.GetOrCreateSettings().CreateWindowsSettings();
            }

            if (parameters.runsOnAndroid)
            {
                AllPlatformSettings.GetOrCreateSettings().CreateAndroidSettings();
            }

            if (parameters.runsOniOS)
            {
                AllPlatformSettings.GetOrCreateSettings().CreateiOSSettings();
            }

            if (parameters.runsOnWebGL)
            {
                AllPlatformSettings.GetOrCreateSettings().CreateWebGLSettings();
            }
        }

        private static void CreateSceneData(SetUpCelesteParameters parameters)
        {
            CreateStartupScene(parameters);
            CreateBootstrapScene(parameters);
            CreateEngineSystemsScene(parameters);
        }

        private static void CreateStartupScene(SetUpCelesteParameters parameters)
        {
            if (parameters.needsStartupScene)
            {
                AssetUtility.CreateFolder("Assets/Startup/Scenes");

                UnityEngine.SceneManagement.Scene startupScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                EditorSceneManager.SaveScene(startupScene, "Assets/Startup/Scenes/Startup.unity");

                SceneSet sceneSet = ScriptableObject.CreateInstance<SceneSet>();
                sceneSet.name = "StartupSceneSet";
                sceneSet.AddScene("Startup", SceneType.Baked);
                AssetUtility.CreateAssetInFolder(sceneSet, "Assets/Startup/Scenes");

                EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
                {
                    new EditorBuildSettingsScene("Assets/Startup/Scenes/Startup.scene", true)
                };

                // Create assembly definition and menu items
                CreateAssembliesParameters startupAssembly = new CreateAssembliesParameters();
                startupAssembly.hasEditorAssembly = true;
                startupAssembly.assemblyName = $"{parameters.rootNamespaceName}.Startup";
                startupAssembly.directoryName = "Startup";
                startupAssembly.hasSceneMenuItem = true;

                CreateAssemblyDefinition.CreateAssemblies(startupAssembly);
            }
        }

        private static void CreateBootstrapScene(SetUpCelesteParameters parameters)
        {
            if (parameters.needsBootstrapScene)
            {
                AssetUtility.CreateFolder("Assets/Bootstrap/Scenes");

                UnityEngine.SceneManagement.Scene bootstrapScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                EditorSceneManager.SaveScene(bootstrapScene, "Assets/Bootstrap/Scenes/Bootstrap.unity");

                SceneSet bootstrapSceneSet = ScriptableObject.CreateInstance<SceneSet>();
                bootstrapSceneSet.name = "BootstrapSceneSet";
                bootstrapSceneSet.AddScene("Bootstrap", SceneType.Addressable);
                AssetUtility.CreateAssetInFolder(bootstrapSceneSet, "Assets/Bootstrap/Scenes");

                // Create assembly definition and menu items
                CreateAssembliesParameters bootstrapAssembly = new CreateAssembliesParameters();
                bootstrapAssembly.hasEditorAssembly = true;
                bootstrapAssembly.assemblyName = $"{parameters.rootNamespaceName}.Bootstrap";
                bootstrapAssembly.directoryName = "Bootstrap";
                bootstrapAssembly.hasSceneMenuItem = true;

                CreateAssemblyDefinition.CreateAssemblies(bootstrapAssembly);
            }
        }

        private static void CreateEngineSystemsScene(SetUpCelesteParameters parameters)
        {
            if (parameters.needsEngineSystemsScene)
            {
                AssetUtility.CreateFolder("Assets/EngineSystems/Scenes");
                
                UnityEngine.SceneManagement.Scene engineSystemsScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                GameObject engineSystemsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Celeste/Engine/Prefabs/EngineSystems.prefab");
                PrefabUtility.InstantiatePrefab(engineSystemsPrefab, engineSystemsScene);
                EditorSceneManager.SaveScene(engineSystemsScene, "Assets/EngineSystems/Scenes/EngineSystems.unity");

                SceneSet engineSystemsSceneSet = ScriptableObject.CreateInstance<SceneSet>();
                engineSystemsSceneSet.name = "EngineSystemsSceneSet";
                engineSystemsSceneSet.AddScene("EngineSystems", SceneType.Addressable);
                AssetUtility.CreateAssetInFolder(engineSystemsSceneSet, "Assets/EngineSystems/Scenes");
            }
        }

        private static void CreateAssetData(SetUpCelesteParameters parameters)
        {
            if (parameters.usesAddressables)
            {
                if (!AddressableAssetSettingsDefaultObject.SettingsExists)
                {
                    AddressableAssetSettingsDefaultObject.GetSettings(true);
                }
            }

            if (parameters.usesTextMeshPro)
            {
                EditorApplication.ExecuteMenuItem(IMPORT_TMPRO_ESSENTIALS_MENU_ITEM);
            }
        }

        #endregion
    }
}
