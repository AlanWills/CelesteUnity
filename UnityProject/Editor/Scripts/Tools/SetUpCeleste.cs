using Celeste.Loading;
using Celeste.Scene;
using Celeste.Startup;
using Celeste.Tools.Attributes.GUI;
using CelesteEditor.BuildSystem;
using CelesteEditor.Tools;
using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.SceneManagement;
using UnityEngine;
using CelesteEditor.UnityProject.Constants;
using Celeste.Bootstrap;
using TMPro.EditorUtilities;
using System.IO;

namespace CelesteEditor.UnityProject
{
    [Serializable]
    public struct SetUpCelesteParameters
    {
        #region Properties and Fields

        [Header("Project")]
        public bool usePresetGitIgnoreFile;
        public bool usePresetGitLFSFile;

        [Header("Build System")]
        public bool needsBuildSystem;
        [ShowIf(nameof(needsBuildSystem))] public bool runsOnWindows;
        [ShowIf(nameof(needsBuildSystem))] public bool runsOnAndroid;
        [ShowIf(nameof(needsBuildSystem))] public bool runsOniOS;
        [ShowIf(nameof(needsBuildSystem))] public bool runsOnWebGL;

        [Header("Code")]
        public string rootNamespaceName;
        public string rootMenuItemName;

        [Header("Assets")]
        public bool usesAddressables;
        [ShowIf(nameof(usesAddressables))] public bool usesBakedGroupsWithRemoteOverride;
        public bool usesTextMeshPro;

        [Header("Scenes")]
        public bool needsStartupScene;
        public bool needsBootstrapScene;
        public bool needsEngineSystemsScene;

        #endregion

        public void UseDefaults()
        {
            usePresetGitIgnoreFile = true;
            usePresetGitLFSFile = true;

            needsBuildSystem = true;
            runsOnWindows = true;
            runsOnAndroid = true;
            runsOniOS = true;
            runsOnWebGL = true;

            usesAddressables = true;
            usesBakedGroupsWithRemoteOverride = true;
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
            CreateProjectData(parameters);
            CreateAssetData(parameters);
            CreateBuildSystemData(parameters);
            CreateModules(parameters);
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

        private static void CreateModules(SetUpCelesteParameters parameters)
        {
            CreateEngineSystems(parameters);
            CreateBootstrap(parameters);
            CreateStartup(parameters);
        }

        #region Startup

        private static void CreateStartup(SetUpCelesteParameters parameters)
        {
            if (parameters.needsStartupScene)
            {
                CreateStartupFolders();
                CreateStartupLoadJob(parameters);
                CreateStartupScene();
                CreateStartupAssemblies(parameters);
            }
        }

        private static void CreateStartupFolders()
        {
            AssetUtility.CreateFolder(StartupConstants.SCENES_FOLDER_PATH);
            AssetUtility.CreateFolder(StartupConstants.LOAD_JOBS_FOLDER_PATH);
        }

        private static void CreateStartupLoadJob(SetUpCelesteParameters parameters)
        {
            var startupLoadJobBuilder = new MultiLoadJob.Builder();

            // Disable fallback assets load job
            {
                var disableFallbackAssets = AssetUtility.FindAsset<LoadJob>(CelesteConstants.DISABLE_FALLBACK_LOAD_ASSETS_LOAD_JOB_NAME);
                Debug.Assert(disableFallbackAssets != null, $"Could not find disable fallback load assets load job: {CelesteConstants.DISABLE_FALLBACK_LOAD_ASSETS_LOAD_JOB_NAME}.");
                startupLoadJobBuilder.WithLoadJob(disableFallbackAssets);
            }

            if (parameters.usesAddressables && parameters.usesBakedGroupsWithRemoteOverride)
            {
                // Load content catalogue load job
                {
                    LoadContentCatalogueLoadJob loadContentCatalogue = ScriptableObject.CreateInstance<LoadContentCatalogueLoadJob>();
                    loadContentCatalogue.name = StartupConstants.LOAD_CONTENT_CATALOGUE_LOAD_JOB_NAME;
                    startupLoadJobBuilder.WithLoadJob(loadContentCatalogue);

                    AssetUtility.CreateAssetInFolder(loadContentCatalogue, StartupConstants.LOAD_JOBS_FOLDER_PATH);
                }

                // Enable bundled addressables load job
                {
                    EnableBundledAddressablesLoadJob enableBundledAddressables = ScriptableObject.CreateInstance<EnableBundledAddressablesLoadJob>();
                    enableBundledAddressables.name = StartupConstants.ENABLE_BUNDLED_ADDRESSABLES_LOAD_JOB_NAME;
                    startupLoadJobBuilder.WithLoadJob(enableBundledAddressables);
                    
                    AssetUtility.CreateAssetInFolder(enableBundledAddressables, StartupConstants.LOAD_JOBS_FOLDER_PATH);
                }
            }

            // Load bootstrap scene set load job
            {
                SceneSet bootstrapSceneSet = AssetUtility.FindAsset<SceneSet>(BootstrapConstants.SCENE_SET_NAME);
                Debug.Assert(bootstrapSceneSet != null, $"Could not find bootstrap scene set for load job: {BootstrapConstants.SCENE_SET_NAME}.  It will have to be set manually later, after the scene set is created.");
                var loadBootstrapSceneSetBuilder = new LoadSceneSetLoadJob.Builder()
                    .WithLoadSceneMode(UnityEngine.SceneManagement.LoadSceneMode.Single)
                    .WithSceneSet(bootstrapSceneSet);

                LoadSceneSetLoadJob loadBootstrapSceneSet = loadBootstrapSceneSetBuilder.Build();
                loadBootstrapSceneSet.name = StartupConstants.LOAD_BOOTSTRAP_SCENE_SET_LOAD_JOB_NAME;
                startupLoadJobBuilder.WithLoadJob(loadBootstrapSceneSet);

                AssetUtility.CreateAssetInFolder(loadBootstrapSceneSet, StartupConstants.LOAD_JOBS_FOLDER_PATH);
            }

            LoadJob startupLoadJob = startupLoadJobBuilder.Build();
            startupLoadJob.name = StartupConstants.LOAD_JOB_NAME;
            AssetUtility.CreateAssetInFolder(startupLoadJob, StartupConstants.LOAD_JOBS_FOLDER_PATH);
        }

        private static void CreateStartupScene()
        {
            UnityEngine.SceneManagement.Scene startupScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            
            // Create main camera
            {
                GameObject cameraGameObject = new GameObject("Main Camera");
                Camera camera = cameraGameObject.AddComponent<Camera>();
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = Color.white;
                camera.orthographic = true;
            }

            // Create startup load component
            {
                GameObject startupLoadGameObject = new GameObject(nameof(StartupLoad));
                StartupLoad startupLoad = startupLoadGameObject.AddComponent<StartupLoad>();
                LoadJob startupLoadJob = AssetUtility.FindAsset<LoadJob>(StartupConstants.LOAD_JOB_NAME);
                Debug.Assert(startupLoadJob != null, $"Could not find startup load job: {StartupConstants.LOAD_JOB_NAME}.  It will have to be set after it is created.");
                startupLoad.StartupLoadJob = startupLoadJob;
            }

            EditorSceneManager.SaveScene(startupScene, StartupConstants.SCENE_PATH);

            SceneSet sceneSet = ScriptableObject.CreateInstance<SceneSet>();
            sceneSet.name = StartupConstants.SCENE_SET_NAME;
            sceneSet.AddScene(StartupConstants.SCENE_NAME, SceneType.Baked);
            AssetUtility.CreateAssetInFolder(sceneSet, StartupConstants.SCENES_FOLDER_PATH);

            EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene(StartupConstants.SCENE_PATH, true)
            };
        }

        private static void CreateStartupAssemblies(SetUpCelesteParameters parameters)
        {
            CreateAssembliesParameters startupAssembly = new CreateAssembliesParameters();
            startupAssembly.hasEditorAssembly = true;
            startupAssembly.assemblyName = $"{parameters.rootNamespaceName}.{StartupConstants.NAMESPACE_NAME}";
            startupAssembly.directoryName = StartupConstants.FOLDER_NAME;
            startupAssembly.hasSceneMenuItem = true;
            startupAssembly.sceneSetPath = $"{StartupConstants.SCENES_FOLDER_PATH}{StartupConstants.SCENE_SET_NAME}.asset";
            startupAssembly.sceneMenuItemPath = $"{parameters.rootMenuItemName}/Scenes/Load {StartupConstants.SCENE_NAME}";

            CreateAssemblyDefinition.CreateAssemblies(startupAssembly); 
        }

        #endregion

        #region Bootstrap

        private static void CreateBootstrap(SetUpCelesteParameters parameters)
        {
            if (parameters.needsBootstrapScene)
            {
                CreateBootstrapFolders();
                CreateBootstrapLoadJob();
                CreateBootstrapScene();
                CreateBootstrapAssemblies(parameters);
            }
        }

        private static void CreateBootstrapFolders()
        {
            AssetUtility.CreateFolder(BootstrapConstants.SCENES_FOLDER_PATH);
            AssetUtility.CreateFolder(BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
        }

        private static void CreateBootstrapLoadJob()
        {
            var bootstrapLoadJobBuilder = new MultiLoadJob.Builder();

            // Load engine systems scene set load job
            {
                SceneSet engineSystemsSceneSet = AssetUtility.FindAsset<SceneSet>(EngineSystemsConstants.SCENE_SET_NAME);
                Debug.Assert(engineSystemsSceneSet != null, $"Could not find engine systems scene set for load job: {EngineSystemsConstants.SCENE_SET_NAME}.  It will have to be set manually later, after the scene set is created.");
                var loadEngineSystemsSceneSetBuilder = new LoadSceneSetLoadJob.Builder()
                    .WithLoadSceneMode(UnityEngine.SceneManagement.LoadSceneMode.Single)
                    .WithSceneSet(engineSystemsSceneSet);

                LoadSceneSetLoadJob loadEngineSystemsSceneSet = loadEngineSystemsSceneSetBuilder.Build();
                loadEngineSystemsSceneSet.name = BootstrapConstants.LOAD_ENGINE_SYSTEMS_SCENE_SET_LOAD_JOB_NAME;
                bootstrapLoadJobBuilder.WithLoadJob(loadEngineSystemsSceneSet);

                AssetUtility.CreateAssetInFolder(loadEngineSystemsSceneSet, BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
                loadEngineSystemsSceneSet.MakeAddressable();
            }

            LoadJob bootstrapLoadJob = bootstrapLoadJobBuilder.Build();
            bootstrapLoadJob.name = BootstrapConstants.LOAD_JOB_NAME;
            
            AssetUtility.CreateAssetInFolder(bootstrapLoadJob, BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
            bootstrapLoadJob.MakeAddressable();
        }

        private static void CreateBootstrapScene()
        {
            UnityEngine.SceneManagement.Scene bootstrapScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject bootstrapManagerPrefab = AssetUtility.FindAsset<GameObject>(BootstrapConstants.BOOTSTRAP_MANAGER_PREFAB_NAME);
            Debug.Assert(bootstrapManagerPrefab != null, $"Could not find bootstrap manager prefab: {BootstrapConstants.BOOTSTRAP_MANAGER_PREFAB_NAME}.");
            
            GameObject bootstrapManagerInstance = PrefabUtility.InstantiatePrefab(bootstrapManagerPrefab, bootstrapScene) as GameObject;
            LoadJob bootstrapLoadJob = AssetUtility.FindAsset<LoadJob>(BootstrapConstants.LOAD_JOB_NAME);
            Debug.Assert(bootstrapLoadJob != null, $"Could not find bootstrap load job: {BootstrapConstants.LOAD_JOB_NAME}.  It will have to be set manually after it is created.");
            bootstrapManagerInstance.GetComponent<BootstrapManager>().bootstrapJob = bootstrapLoadJob;
            EditorSceneManager.SaveScene(bootstrapScene, BootstrapConstants.SCENE_PATH);
            AssetDatabase.LoadAssetAtPath<SceneAsset>(BootstrapConstants.SCENE_PATH).SetAddressableAddress(BootstrapConstants.SCENE_NAME);

            SceneSet bootstrapSceneSet = ScriptableObject.CreateInstance<SceneSet>();
            bootstrapSceneSet.name = BootstrapConstants.SCENE_SET_NAME;
            bootstrapSceneSet.AddScene(BootstrapConstants.SCENE_NAME, SceneType.Addressable);
            bootstrapSceneSet.AddScene(CelesteConstants.LOADING_SCENE_NAME, SceneType.Addressable);
            
            AssetUtility.CreateAssetInFolder(bootstrapSceneSet, BootstrapConstants.SCENES_FOLDER_PATH);
            bootstrapSceneSet.MakeAddressable();
        }

        private static void CreateBootstrapAssemblies(SetUpCelesteParameters parameters)
        {
            CreateAssembliesParameters bootstrapAssembly = new CreateAssembliesParameters();
            bootstrapAssembly.hasEditorAssembly = true;
            bootstrapAssembly.assemblyName = $"{parameters.rootNamespaceName}.{BootstrapConstants.NAMESPACE_NAME}";
            bootstrapAssembly.directoryName = BootstrapConstants.FOLDER_NAME;
            bootstrapAssembly.hasSceneMenuItem = true;
            bootstrapAssembly.sceneSetPath = $"{BootstrapConstants.SCENES_FOLDER_PATH}{BootstrapConstants.SCENE_SET_NAME}.asset";
            bootstrapAssembly.sceneMenuItemPath = $"{parameters.rootMenuItemName}/Scenes/Load {BootstrapConstants.SCENE_NAME}";

            CreateAssemblyDefinition.CreateAssemblies(bootstrapAssembly);
        }

        #endregion

        #region Engine Systems

        private static void CreateEngineSystems(SetUpCelesteParameters parameters)
        {
            if (parameters.needsEngineSystemsScene)
            {
                CreateEngineSystemsFolders();
                CreateEngineSystemsScene();
            }
        }

        private static void CreateEngineSystemsFolders()
        {
            AssetUtility.CreateFolder(EngineSystemsConstants.SCENES_FOLDER_PATH);
        }

        private static void CreateEngineSystemsScene()
        {
            UnityEngine.SceneManagement.Scene engineSystemsScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject engineSystemsPrefab = AssetUtility.FindAsset<GameObject>(EngineSystemsConstants.ENGINE_SYSTEMS_PREFAB_NAME);
            PrefabUtility.InstantiatePrefab(engineSystemsPrefab, engineSystemsScene);
            EditorSceneManager.SaveScene(engineSystemsScene, EngineSystemsConstants.SCENE_PATH);
            AssetDatabase.LoadAssetAtPath<SceneAsset>(EngineSystemsConstants.SCENE_PATH).SetAddressableAddress(EngineSystemsConstants.SCENE_NAME);

            SceneSet engineSystemsSceneSet = ScriptableObject.CreateInstance<SceneSet>();
            engineSystemsSceneSet.name = EngineSystemsConstants.SCENE_SET_NAME;
            engineSystemsSceneSet.AddScene(EngineSystemsConstants.SCENE_NAME, SceneType.Addressable);
            
            AssetUtility.CreateAssetInFolder(engineSystemsSceneSet, EngineSystemsConstants.SCENES_FOLDER_PATH);
            engineSystemsSceneSet.MakeAddressable();
        }

        #endregion

        private static void CreateProjectData(SetUpCelesteParameters parameters)
        {
            string projectPath = Path.GetDirectoryName(Application.dataPath);

            if (parameters.usePresetGitIgnoreFile)
            {
                try
                {
                    File.Move(CelesteConstants.CELESTE_GIT_IGNORE_FILE_PATH, Path.Combine(projectPath, ".gitignore"));
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to create preset git ignore file.  See the log for an exception.");
                    Debug.LogException(ex);
                }
            }

            if (parameters.usePresetGitLFSFile)
            {
                try
                {
                    File.Move(CelesteConstants.CELESTE_GIT_LFS_FILE_PATH, Path.Combine(projectPath, ".gitattributes"));
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to create preset git lfs file.  See the log for an exception.");
                    Debug.LogException(ex);
                }
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
                string packageFullPath = TMP_EditorUtility.packageFullPath;
                AssetDatabase.ImportPackage($"{packageFullPath}/Package Resources/TMP Essential Resources.unitypackage", false);
            }
        }

        #endregion
    }
}
