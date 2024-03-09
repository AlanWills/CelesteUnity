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
using CelesteEditor.Scene.Settings;
using Celeste.Sound.Settings;
using Celeste.Persistence.Settings;
using Celeste.Localisation.Settings;
using Celeste.LiveOps.Settings;
using Celeste.Debug.Settings;
using Celeste.DataImporters.Settings;
using Celeste.Input.Settings;
using UnityEditor.AddressableAssets.Settings;
using CelesteEditor.Assets;
using CelesteEditor.Assets.Schemas;
using CelesteEditor.BuildSystem.Data;
using CelesteEditor.Scene;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine.SceneManagement;
using Celeste.Sound;
using UnityEditorInternal;

namespace CelesteEditor.UnityProject
{
    [Serializable]
    public struct SetUpCelesteParameters
    {
        #region Properties and Fields

        public CelesteConstants CelesteConstants => new CelesteConstants(packagePath);
        public BuildSystemConstants BuildSystemConstants => new BuildSystemConstants(packagePath);

        [Header("Project")]
        [LabelWidth(300)]
        [Tooltip("The path to the root of the Celeste package.  This only needs modifying if you've manually cloned the repo into your project.")]
        public string packagePath;
        [LabelWidth(300)]
        [Tooltip("If true, Celeste will be embedded as a local package rather than referenced as a read-only package.  Useful if you want to edit the source or utilise assets in the package.")]
        public bool embedCeleste;
        [LabelWidth(300)] public bool usePresetGitIgnoreFile;
        [LabelWidth(300)] public bool usePresetGitLFSFile;

        [Header("Dependencies")]
        [LabelWidth(300)] public bool useNativeSharePackage;
        [LabelWidth(300)] public bool useNativeFilePickerPackage;
        [LabelWidth(300)] public List<string> dependencies;

        [Header("Build System")]
        [LabelWidth(300)] public bool needsBuildSystem;
        [LabelWidth(300), ShowIf(nameof(needsBuildSystem))] public bool runsOnWindows;
        [LabelWidth(300), ShowIf(nameof(needsBuildSystem))] public bool runsOnAndroid;
        [LabelWidth(300), ShowIf(nameof(needsBuildSystem))] public bool runsOniOS;
        [LabelWidth(300), ShowIf(nameof(needsBuildSystem))] public bool runsOnWebGL;
        [LabelWidth(300), ShowIf(nameof(needsBuildSystem))]
        [Tooltip("If true, copies of Common template jenkins files will be added to the project for customisation and usage")]
        public bool useCommonJenkinsFiles;
        [LabelWidth(300), ShowIfAll(nameof(needsBuildSystem), nameof(runsOnWindows))]
        [Tooltip("If true, copies of Windows template jenkins files will be added to the project for customisation and usage")]
        public bool useWindowsBuildJenkinsFiles;
        [LabelWidth(300), ShowIfAll(nameof(needsBuildSystem), nameof(runsOnAndroid))]
        [Tooltip("If true, copies of the Android template jenkins files will be added to the project for customisation and usage")]
        public bool useAndroidBuildJenkinsFiles;
        [LabelWidth(300), ShowIfAll(nameof(needsBuildSystem), nameof(runsOniOS))]
        [Tooltip("If true, copies of the iOS template jenkins files will be added to the project for customisation and usage")]
        public bool useiOSBuildJenkinsFiles;
        [LabelWidth(300), ShowIfAll(nameof(needsBuildSystem), nameof(runsOnWebGL))]
        [Tooltip("If true, copies of the WebGL template jenkins files will be added to the project for customisation and usage")]
        public bool useWebGLBuildJenkinsFiles;

        [Header("Code")]
        [LabelWidth(300)] public string rootNamespaceName;
        [LabelWidth(300)] public string rootMenuItemName;

        [Header("Assets")]
        [LabelWidth(300)] public bool usesAddressables;
        [LabelWidth(300), ShowIf(nameof(usesAddressables))] public bool usesBakedGroupsWithRemoteOverride;
        [LabelWidth(300)] public bool usesTextMeshPro;

        [Header("Scenes")]
        [LabelWidth(300)] public bool needsStartupScene;
        [LabelWidth(300)] public bool needsBootstrapScene;
        [LabelWidth(300)] public bool needsEngineSystemsScene;
        [LabelWidth(300)] public bool needsGameSystemsScene;

        #endregion

        public void UseDefaults()
        {
            packagePath = "Packages/com.celestegames.celeste/";
            embedCeleste = true;

            usePresetGitIgnoreFile = true;
            usePresetGitLFSFile = true;

            useNativeSharePackage = true;
            useNativeFilePickerPackage = true;
            dependencies = new List<string>();

            needsBuildSystem = true;
            runsOnWindows = true;
            runsOnAndroid = true;
            runsOniOS = true;
            runsOnWebGL = true;
            useCommonJenkinsFiles = true;
            useWindowsBuildJenkinsFiles = true;
            useAndroidBuildJenkinsFiles = true;
            useiOSBuildJenkinsFiles = true;
            useWebGLBuildJenkinsFiles = true;

            usesAddressables = true;
            usesBakedGroupsWithRemoteOverride = true;
            usesTextMeshPro = true;

            needsStartupScene = true;
            needsBootstrapScene = true;
            needsEngineSystemsScene = true;
            needsGameSystemsScene = true;
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
            CreateEditorSettings(parameters);
            CreateProjectData(parameters);
            CreateBuildSystemData(parameters);
            CreateModules(parameters);
            CreateFileShareSettings();

            Finalise(parameters);
        }

        private static void Finalise(SetUpCelesteParameters parameters)
        {
            if (parameters.embedCeleste)
            {
                if (EmbedPackage.CanEmbed(parameters.packagePath))
                {
                    EmbedPackage.Embed(parameters.packagePath);
                }
                else
                {
                    Debug.LogError($"Failed to embed Celeste as a package (package path: {parameters.packagePath})");
                }
            }
            
            CelesteSceneMenuItems.UpdateScenesInBuild();

            List<string> dependencies = new List<string>(parameters.dependencies);

            if (parameters.useNativeFilePickerPackage)
            {
                dependencies.Add("git@github.com:AlanWills/UnityNativeFilePicker.git");
            }

            if (parameters.useNativeSharePackage)
            {
                dependencies.Add("git@github.com:AlanWills/UnityNativeShare.git");
            }

            if (dependencies.Count > 0)
            {
                Client.AddAndRemove(packagesToAdd: dependencies.ToArray());
            }
        }


        private static void CreateModules(SetUpCelesteParameters parameters)
        {
            CreateGameSystems(parameters);
            CreateEngineSystems(parameters);
            CreateBootstrap(parameters);
            CreateStartup(parameters);
        }

        private static void CreateProjectData(SetUpCelesteParameters parameters)
        {
            string projectPath = Path.GetDirectoryName(Application.dataPath);

            if (parameters.usePresetGitIgnoreFile)
            {
                try
                {
                    File.Copy(parameters.CelesteConstants.CELESTE_GIT_IGNORE_FILE_PATH, Path.Combine(projectPath, ".gitignore"));
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
                    File.Copy(parameters.CelesteConstants.CELESTE_GIT_LFS_FILE_PATH, Path.Combine(projectPath, ".gitattributes"));
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to create preset git lfs file.  See the log for an exception.");
                    Debug.LogException(ex);
                }
            }
        }

        #region Build System

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

            if (parameters.useCommonJenkinsFiles)
            {
                AssetUtility.CreateFolder(BuildSystemConstants.FOLDER_PATH);

                CopyDirectoryRecursively(parameters.BuildSystemConstants.CELESTE_COMMON_JENKINS_BUILD_FILES_FOLDER, BuildSystemConstants.COMMON_JENKINS_BUILD_FILES_FOLDER);
            }

            if (parameters.useWindowsBuildJenkinsFiles)
            {
                CopyDirectoryRecursively(parameters.BuildSystemConstants.CELESTE_WINDOWS_JENKINS_BUILD_FILES_FOLDER, BuildSystemConstants.WINDOWS_JENKINS_BUILD_FILES_FOLDER);
            }

            if (parameters.useAndroidBuildJenkinsFiles)
            {
                CopyDirectoryRecursively(parameters.BuildSystemConstants.CELESTE_ANDROID_JENKINS_BUILD_FILES_FOLDER, BuildSystemConstants.ANDROID_JENKINS_BUILD_FILES_FOLDER);
            }

            if (parameters.useiOSBuildJenkinsFiles)
            {
                CopyDirectoryRecursively(parameters.BuildSystemConstants.CELESTE_IOS_JENKINS_BUILD_FILES_FOLDER, BuildSystemConstants.IOS_JENKINS_BUILD_FILES_FOLDER);
            }

            if (parameters.useWebGLBuildJenkinsFiles)
            {
                CopyDirectoryRecursively(parameters.BuildSystemConstants.CELESTE_WEBGL_JENKINS_BUILD_FILES_FOLDER, BuildSystemConstants.WEBGL_JENKINS_BUILD_FILES_FOLDER);
            }

            // Create Scripting Define Symbols
            AssetUtility.CreateFolder(BuildSystemConstants.SCRIPTING_DEFINE_SYMBOLS_FOLDER);

            // Debug
            {
                ScriptingDefineSymbols debugScriptingDefineSymbols = ScriptableObject.CreateInstance<ScriptingDefineSymbols>();
                debugScriptingDefineSymbols.name = "DebugScriptingDefineSymbols";
                debugScriptingDefineSymbols.AddDefaultDebugSymbols();
                AssetUtility.CreateAssetInFolder(debugScriptingDefineSymbols, BuildSystemConstants.SCRIPTING_DEFINE_SYMBOLS_FOLDER);
            }

            // Release
            {
                ScriptingDefineSymbols debugScriptingDefineSymbols = ScriptableObject.CreateInstance<ScriptingDefineSymbols>();
                debugScriptingDefineSymbols.name = "ReleaseScriptingDefineSymbols";
                debugScriptingDefineSymbols.AddDefaultReleaseSymbols();
                AssetUtility.CreateAssetInFolder(debugScriptingDefineSymbols, BuildSystemConstants.SCRIPTING_DEFINE_SYMBOLS_FOLDER);
            }
        }

        #endregion

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
            var startupLoadJobBuilder = new MultiLoadJob.Builder()
                .WithShowOutputInLoadingScreen(false);

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
                    .WithLoadSceneMode(LoadSceneMode.Single)
                    .WithSceneSet(bootstrapSceneSet)
                    .WithShowOutputOnLoadingScreen(false);

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
            sceneSet.AddScene(StartupConstants.SCENE_NAME, SceneType.Baked, false);
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
            startupAssembly.createSceneSet = false;

            CreateAssemblyDefinition.CreateAssemblies(startupAssembly);
        }

        #endregion

        #region Bootstrap

        private static void CreateBootstrap(SetUpCelesteParameters parameters)
        {
            if (parameters.needsBootstrapScene)
            {
                CreateBootstrapFolders();
                CreateBootstrapLoadJob(parameters);
                CreateBootstrapScene(parameters);
                CreateBootstrapAssemblies(parameters);
            }
        }

        private static void CreateBootstrapFolders()
        {
            AssetUtility.CreateFolder(BootstrapConstants.SCENES_FOLDER_PATH);
            AssetUtility.CreateFolder(BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
        }

        private static void CreateBootstrapLoadJob(SetUpCelesteParameters parameters)
        {
            var bootstrapLoadJobBuilder = new MultiLoadJob.Builder()
                .WithShowOutputInLoadingScreen(false);

            // Disable fallback assets load job
            {
                var disableFallbackAssets = AssetUtility.FindAsset<LoadJob>(CelesteConstants.DISABLE_FALLBACK_LOAD_ASSETS_LOAD_JOB_NAME);
                Debug.Assert(disableFallbackAssets != null, $"Could not find disable fallback load assets load job: {CelesteConstants.DISABLE_FALLBACK_LOAD_ASSETS_LOAD_JOB_NAME}.");
                bootstrapLoadJobBuilder.WithLoadJob(disableFallbackAssets);
                disableFallbackAssets.MakeAddressable();
            }

            // Load engine systems scene set load job
            {
                SceneSet engineSystemsSceneSet = AssetUtility.FindAsset<SceneSet>(EngineSystemsConstants.SCENE_SET_NAME);
                Debug.Assert(engineSystemsSceneSet != null, $"Could not find engine systems scene set for load job: {EngineSystemsConstants.SCENE_SET_NAME}.  It will have to be set manually later, after the scene set is created.");
                var loadEngineSystemsSceneSetBuilder = new LoadSceneSetLoadJob.Builder()
                    .WithLoadSceneMode(LoadSceneMode.Additive)
                    .WithSceneSet(engineSystemsSceneSet)
                    .WithShowOutputOnLoadingScreen(false);

                LoadSceneSetLoadJob loadEngineSystemsSceneSet = loadEngineSystemsSceneSetBuilder.Build();
                loadEngineSystemsSceneSet.name = BootstrapConstants.LOAD_ENGINE_SYSTEMS_SCENE_SET_LOAD_JOB_NAME;
                bootstrapLoadJobBuilder.WithLoadJob(loadEngineSystemsSceneSet);

                AssetUtility.CreateAssetInFolder(loadEngineSystemsSceneSet, BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
                MakeAddressable(parameters, loadEngineSystemsSceneSet);
            }

            LoadJob bootstrapLoadJob = bootstrapLoadJobBuilder.Build();
            bootstrapLoadJob.name = BootstrapConstants.LOAD_JOB_NAME;

            AssetUtility.CreateAssetInFolder(bootstrapLoadJob, BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
            MakeAddressable(parameters, bootstrapLoadJob);
        }

        private static void CreateBootstrapScene(SetUpCelesteParameters parameters)
        {
            UnityEngine.SceneManagement.Scene bootstrapScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject bootstrapManagerPrefab = AssetUtility.FindAsset<GameObject>(BootstrapConstants.BOOTSTRAP_MANAGER_PREFAB_NAME);
            Debug.Assert(bootstrapManagerPrefab != null, $"Could not find bootstrap manager prefab: {BootstrapConstants.BOOTSTRAP_MANAGER_PREFAB_NAME}.");

            GameObject bootstrapManagerInstance = PrefabUtility.InstantiatePrefab(bootstrapManagerPrefab, bootstrapScene) as GameObject;
            LoadJob bootstrapLoadJob = AssetUtility.FindAsset<LoadJob>(BootstrapConstants.LOAD_JOB_NAME);
            Debug.Assert(bootstrapLoadJob != null, $"Could not find bootstrap load job: {BootstrapConstants.LOAD_JOB_NAME}.  It will have to be set manually after it is created.");
            bootstrapManagerInstance.GetComponent<BootstrapManager>().bootstrapJob = bootstrapLoadJob;
            EditorUtility.SetDirty(bootstrapManagerInstance);
            EditorSceneManager.SaveScene(bootstrapScene, BootstrapConstants.SCENE_PATH);
            AssetDatabase.LoadAssetAtPath<SceneAsset>(parameters.CelesteConstants.LOADING_SCENE_PATH).SetAddressableAddress(CelesteConstants.LOADING_SCENE_NAME);
            AssetDatabase.LoadAssetAtPath<SceneAsset>(BootstrapConstants.SCENE_PATH).SetAddressableAddress(BootstrapConstants.SCENE_NAME);

            SceneSet bootstrapSceneSet = ScriptableObject.CreateInstance<SceneSet>();
            bootstrapSceneSet.name = BootstrapConstants.SCENE_SET_NAME;
            bootstrapSceneSet.AddScene(CelesteConstants.LOADING_SCENE_NAME, parameters.usesAddressables ? SceneType.Addressable : SceneType.Baked, false); // This must be first
            bootstrapSceneSet.AddScene(BootstrapConstants.SCENE_NAME, parameters.usesAddressables ? SceneType.Addressable : SceneType.Baked, false);
            bootstrapSceneSet.HasCustomDebugBuildValue = false;

            AssetUtility.CreateAssetInFolder(bootstrapSceneSet, BootstrapConstants.SCENES_FOLDER_PATH);
            MakeAddressable(parameters, bootstrapSceneSet);
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
            bootstrapAssembly.createSceneSet = false;

            CreateAssemblyDefinition.CreateAssemblies(bootstrapAssembly);
        }

        #endregion

        #region Engine Systems

        private static void CreateEngineSystems(SetUpCelesteParameters parameters)
        {
            if (parameters.needsEngineSystemsScene)
            {
                CreateEngineSystemsFolders();
                CreateEngineSystemsScenes(parameters);
                CreateEngineSystemsAssets(parameters);
            }
        }

        private static void CreateEngineSystemsFolders()
        {
            AssetUtility.CreateFolder(EngineSystemsConstants.SCENES_FOLDER_PATH);
        }

        private static void CreateEngineSystemsScenes(SetUpCelesteParameters parameters)
        {
            // Create Engine Systems Scene
            {
                CreateSceneWithPrefab(parameters, EngineSystemsConstants.SCENE_NAME, EngineSystemsConstants.SCENE_PATH, EngineSystemsConstants.ENGINE_SYSTEMS_PREFAB_NAME);
            }

            // Create Engine Systems Debug Scene
            {
                CreateSceneWithPrefab(parameters, EngineSystemsConstants.DEBUG_SCENE_NAME, EngineSystemsConstants.DEBUG_SCENE_PATH, EngineSystemsConstants.ENGINE_SYSTEMS_DEBUG_PREFAB_NAME);
            }

            SceneSet engineSystemsSceneSet = ScriptableObject.CreateInstance<SceneSet>();
            engineSystemsSceneSet.name = EngineSystemsConstants.SCENE_SET_NAME;
            engineSystemsSceneSet.AddScene(EngineSystemsConstants.SCENE_NAME, parameters.usesAddressables ? SceneType.Addressable : SceneType.Baked, false);
            engineSystemsSceneSet.AddScene(EngineSystemsConstants.DEBUG_SCENE_NAME, parameters.usesAddressables ? SceneType.Addressable : SceneType.Baked, true);

            AssetUtility.CreateAssetInFolder(engineSystemsSceneSet, EngineSystemsConstants.SCENES_FOLDER_PATH);
            MakeAddressable(parameters, engineSystemsSceneSet);
        }

        private static void CreateEngineSystemsAssets(SetUpCelesteParameters parameters)
        {
            MusicSettings musicSettings = ScriptableObject.CreateInstance<MusicSettingsUsingAssets>();
            musicSettings.name = "MusicSettings";
            MakeAddressable(parameters, musicSettings);

            SFXSettings sfxSettings = ScriptableObject.CreateInstance<SFXSettingsUsingAssets>();
            sfxSettings.name = "SFXSettings";
            MakeAddressable(parameters, sfxSettings);

            AssetUtility.CreateAssetInFolder(musicSettings, EngineSystemsConstants.DATA_FOLDER_PATH);
            AssetUtility.CreateAssetInFolder(sfxSettings, EngineSystemsConstants.DATA_FOLDER_PATH);

            SceneAsset engineSystemsScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EngineSystemsConstants.SCENE_PATH);
            SceneManager.LoadScene(engineSystemsScene.name, LoadSceneMode.Single);

            // Set Music Settings
            {
                MusicManager musicManager = GameObject.FindFirstObjectByType<MusicManager>();
                Debug.Assert(musicManager, $"Could not find {nameof(MusicManager)} in the Engine Systems scene!  You'll have to assign Music Settings manually...");

                if (musicManager != null)
                {
                    musicManager.MusicSettings = musicSettings;
                }
            }

            // Set SFX Settings
            {
                SFXManager sfxManager = GameObject.FindFirstObjectByType<SFXManager>();
                Debug.Assert(sfxManager, $"Could not find {nameof(SFXManager)} in the Engine Systems scene!  You'll have to assign SFX Settings manually...");

                if (sfxManager != null)
                {
                    sfxManager.SFXSettings = sfxSettings;
                }
            }
        }

        #endregion

        #region Game Systems

        private static void CreateGameSystems(SetUpCelesteParameters parameters)
        {
            if (parameters.needsGameSystemsScene)
            {
                CreateGameSystemsFolders();
                CreateGameSystemsScenes(parameters);
                CreateEngineSystemsAssets(parameters);
            }
        }

        private static void CreateGameSystemsFolders()
        {
            AssetUtility.CreateFolder(GameSystemsConstants.SCENES_FOLDER_PATH);
        }

        private static void CreateGameSystemsScenes(SetUpCelesteParameters parameters)
        {
            // Create Game Systems Scene
            {
                CreateScene(parameters, GameSystemsConstants.SCENE_NAME, GameSystemsConstants.SCENE_PATH);
            }

            // Create Game Systems Debug Scene
            {
                CreateScene(parameters, GameSystemsConstants.DEBUG_SCENE_NAME, GameSystemsConstants.DEBUG_SCENE_PATH);
            }

            SceneSet gameSystemsSceneSet = ScriptableObject.CreateInstance<SceneSet>();
            gameSystemsSceneSet.name = GameSystemsConstants.SCENE_SET_NAME;
            gameSystemsSceneSet.AddScene(GameSystemsConstants.SCENE_NAME, parameters.usesAddressables ? SceneType.Addressable : SceneType.Baked, false);
            gameSystemsSceneSet.AddScene(GameSystemsConstants.DEBUG_SCENE_NAME, parameters.usesAddressables ? SceneType.Addressable : SceneType.Baked, true);

            AssetUtility.CreateAssetInFolder(gameSystemsSceneSet, GameSystemsConstants.SCENES_FOLDER_PATH);
            MakeAddressable(parameters, gameSystemsSceneSet);
        }

        #endregion

        #region Editor Settings

        private static void CreateEditorSettings(SetUpCelesteParameters parameters)
        {
            DataImporterEditorSettings.GetOrCreateSettings();
            DebugEditorSettings.GetOrCreateSettings();
            LiveOpsEditorSettings.GetOrCreateSettings();

            {
                LocalisationEditorSettings localisationEditorSettings = LocalisationEditorSettings.GetOrCreateSettings();
                MakeAddressable(parameters, localisationEditorSettings.currentLanguageValue);
            }

            PersistenceEditorSettings.GetOrCreateSettings();

            {
                SceneEditorSettings sceneEditorSettings = SceneEditorSettings.GetOrCreateSettings();
                MakeAddressable(parameters, sceneEditorSettings.defaultContextProvider);
                MakeAddressable(parameters, sceneEditorSettings.defaultLoadContextEvent);
            }

            {
                SoundEditorSettings soundEditorSettings = SoundEditorSettings.GetOrCreateSettings();
                MakeAddressable(parameters, soundEditorSettings.playMusicWithRawClipEvent);
                MakeAddressable(parameters, soundEditorSettings.playMusicWithSettingsEvent);
                MakeAddressable(parameters, soundEditorSettings.playMusicOneShotWithRawClipEvent);
                MakeAddressable(parameters, soundEditorSettings.playMusicOneShotWithSettingsEvent);
                MakeAddressable(parameters, soundEditorSettings.playSFXWithRawClipEvent);
                MakeAddressable(parameters, soundEditorSettings.playSFXWithSettingsEvent);
                MakeAddressable(parameters, soundEditorSettings.playSFXOneShotWithRawClipEvent);
                MakeAddressable(parameters, soundEditorSettings.playSFXOneShotWithSettingsEvent);
            }

            {
                InputEditorSettings inputEditorSettings = InputEditorSettings.GetOrCreateSettings();

                if (parameters.usesAddressables)
                {
                    MakeAddressable(parameters, inputEditorSettings.InputCamera);
                    MakeAddressable(parameters, inputEditorSettings.LeftMouseButtonDown);
                    MakeAddressable(parameters, inputEditorSettings.LeftMouseButtonFirstDown);
                    MakeAddressable(parameters, inputEditorSettings.LeftMouseButtonFirstUp);
                    MakeAddressable(parameters, inputEditorSettings.MiddleMouseButtonDown);
                    MakeAddressable(parameters, inputEditorSettings.MiddleMouseButtonFirstDown);
                    MakeAddressable(parameters, inputEditorSettings.MiddleMouseButtonFirstUp);
                    MakeAddressable(parameters, inputEditorSettings.RightMouseButtonDown);
                    MakeAddressable(parameters, inputEditorSettings.RightMouseButtonFirstDown);
                    MakeAddressable(parameters, inputEditorSettings.RightMouseButtonFirstUp);
                    MakeAddressable(parameters, inputEditorSettings.SingleTouch);
                    MakeAddressable(parameters, inputEditorSettings.DoubleTouch);
                    MakeAddressable(parameters, inputEditorSettings.TripleTouch);
                }
            }
        }

        #endregion

        #region File Share Settings

        private static void CreateFileShareSettings()
        {
#if CELESTE_NATIVE_FILE_PICKER
            NativeFilePickerNamespace.NativeFilePickerCustomTypes nativeFilePickerCustomTypes = NativeFilePickerNamespace.NativeFilePickerCustomTypes.GetInstance(true);
            nativeFilePickerCustomTypes.AddCustomType(
                new NativeFilePickerNamespace.NativeFilePickerCustomTypes.TypeHolder()
                {
                    identifier = "com.celestegames.datafile",
                    description = "Serialized save data",
                    isExported = true,
                    conformsTo = new string[] { "public.data" },
                    extensions = new string[] { "dat" }
                });
            nativeFilePickerCustomTypes.AddCustomType(
                new NativeFilePickerNamespace.NativeFilePickerCustomTypes.TypeHolder()
                {
                    identifier = "com.celestegames.datasnapshot",
                    description = "All serialized data within the app compressed into a single shareable file",
                    isExported = true,
                    conformsTo = new string[] { "public.data" },
                    extensions = new string[] { "datasnapshot" }
                });
#endif
        }

        #endregion

        #region Utility

        private static void CopyDirectoryRecursively(string originalDirectory, string newDirectory)
        {
            // In case we're working in the packages folder - we need to be able to appropriately copy the file
            originalDirectory = Path.GetFullPath(originalDirectory);
            Debug.Log($"Recursively copying from '{originalDirectory}' to {newDirectory}.");

            if (Directory.Exists(originalDirectory))
            {
                DirectoryInfo originalDirectoryInfo = new DirectoryInfo(originalDirectory);
                AssetUtility.CreateFolder(newDirectory);

                foreach (FileInfo file in originalDirectoryInfo.GetFiles())
                {
                    File.Copy(file.FullName, Path.Combine(newDirectory, file.Name));
                }

                foreach (DirectoryInfo directory in originalDirectoryInfo.GetDirectories())
                {
                    CopyDirectoryRecursively(directory.FullName, Path.Combine(newDirectory, directory.Name));
                }
            }
            else
            {
                Debug.LogError($"Could not find original directory: {originalDirectory} in recursive copy action to {newDirectory}.");
            }
        }

        private static void CreateScene(SetUpCelesteParameters parameters, string sceneName, string scenePath)
        {
            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(scene, scenePath);

            SetAddressableAddress(parameters, scenePath, sceneName);
        }

        private static void CreateSceneWithPrefab(SetUpCelesteParameters parameters, string sceneName, string scenePath, string scenePrefabName)
        {
            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            GameObject prefab = AssetUtility.FindAsset<GameObject>(scenePrefabName);
            PrefabUtility.InstantiatePrefab(prefab, scene);
            EditorSceneManager.SaveScene(scene, scenePath);

            SetAddressableAddress(parameters,scenePath, sceneName);
        }

        private static void CreateAssetData(SetUpCelesteParameters parameters)
        {
            if (parameters.usesAddressables)
            {
                // Set Settings
                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.GetSettings(!AddressableAssetSettingsDefaultObject.SettingsExists);
                settings.BuildRemoteCatalog = true;
                settings.BuildAddressablesWithPlayerBuild = AddressableAssetSettings.PlayerBuildOption.DoNotBuildWithPlayer;
                settings.MaxConcurrentWebRequests = 50;
                settings.OverridePlayerVersion = "res";
                settings.ShaderBundleNaming = UnityEditor.AddressableAssets.Build.ShaderBundleNaming.Custom;
                settings.ShaderBundleCustomNaming = "alwaysbundle";
                bool result = settings.RemoteCatalogBuildPath.SetVariableByName(settings, "Remote");
                Debug.Assert(result, "Failed to set Remote Catalog Build Path to 'Remote'.  This will need to be done manually in the Addressable Settings.");

                // Rename default group and add tag
                settings.AddLabel("CommonAddressables", false);
                settings.DefaultGroup.Name = "CommonAddressables";
                settings.DefaultGroup.AddSchema<BundledGroupSchema>(false);

                string remoteBuildPath = AddressablesUtility.GetAddressablesRemoteBuildPath();
                string remoteLoadPath = AddressablesUtility.GetAddressablesRemoteLoadPath();

                settings.DefaultGroup.SetBuildPath(remoteBuildPath);
                settings.DefaultGroup.SetLoadPath(remoteLoadPath);
            }

            if (parameters.usesTextMeshPro)
            {
                string packageFullPath = TMP_EditorUtility.packageFullPath;
                AssetDatabase.ImportPackage($"{packageFullPath}/Package Resources/TMP Essential Resources.unitypackage", false);
            }
        }

        private static void MakeAddressable(SetUpCelesteParameters parameters, UnityEngine.Object obj)
        {
            if (parameters.usesAddressables)
            {
                obj.MakeAddressable();
            }
        }

        private static void SetAddressableAddress(SetUpCelesteParameters parameters, UnityEngine.Object obj, string address)
        {
            if (parameters.usesAddressables)
            {
                obj.SetAddressableAddress(address);
            }
        }

        private static void SetAddressableAddress(SetUpCelesteParameters parameters, string assetPath, string address)
        {
            if (parameters.usesAddressables)
            {
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath).SetAddressableAddress(address);
            }
        }

        #endregion
    }
}
