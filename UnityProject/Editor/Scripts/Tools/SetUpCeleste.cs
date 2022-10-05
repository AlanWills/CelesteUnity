using Celeste.Scene;
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
        public bool runsOnWindows;
        public bool runsOnAndroid;
        public bool runsOniOS;
        public bool runsOnWebGL;

        [Header("Code")]
        public string rootNamespaceName;

        [Header("Assets")]
        public bool usesAddressables;

        [Header("Scenes")]
        public bool needsStartupScene;
        public bool needsBootstrapScene;

        #endregion

        public void UseDefaults()
        {
            needsBuildSystem = true;
            runsOnWindows = true;
            runsOnAndroid = true;
            runsOniOS = true;
            runsOnWebGL = true;

            usesAddressables = true;

            needsStartupScene = true;
            needsBootstrapScene = true;
        }
    }

    public static class SetUpCeleste
    {
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
            if (parameters.needsStartupScene)
            {
                CreateStartupScene(parameters);
            }

            if (parameters.needsBootstrapScene)
            {
                CreateBootstrapScene(parameters);
            }
        }

        private static void CreateStartupScene(SetUpCelesteParameters parameters)
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

        private static void CreateBootstrapScene(SetUpCelesteParameters parameters)
        {
            AssetUtility.CreateFolder("Assets/Bootstrap/Scenes");

            UnityEngine.SceneManagement.Scene bootstrapScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(bootstrapScene, "Assets/Bootstrap/Scenes/Bootstrap.unity");

            SceneSet bootstrapSceneSet = ScriptableObject.CreateInstance<SceneSet>();
            bootstrapSceneSet.name = "BootstrapSceneSet";
            bootstrapSceneSet.AddScene("Bootstrap", SceneType.Addressable);
            AssetUtility.CreateAssetInFolder(bootstrapSceneSet, "Assets/Bootstrap/Scenes");

            SceneSet engineSystemsSceneSet = ScriptableObject.CreateInstance<SceneSet>();
            engineSystemsSceneSet.name = "EngineSystemsSceneSet";
            engineSystemsSceneSet.AddScene("EngineSystems", SceneType.Addressable);
            AssetUtility.CreateAssetInFolder(engineSystemsSceneSet, "Assets/Bootstrap/Scenes");

            // Create assembly definition and menu items
            CreateAssembliesParameters bootstrapAssembly = new CreateAssembliesParameters();
            bootstrapAssembly.hasEditorAssembly = true;
            bootstrapAssembly.assemblyName = $"{parameters.rootNamespaceName}.Bootstrap";
            bootstrapAssembly.directoryName = "Bootstrap";
            bootstrapAssembly.hasSceneMenuItem = true;

            CreateAssemblyDefinition.CreateAssemblies(bootstrapAssembly);
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
        }

        #endregion
    }
}
