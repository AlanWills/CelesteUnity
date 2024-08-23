using Celeste.Loading;
using Celeste.Scene;
using Celeste.Startup;
using Celeste.Tools.Attributes.GUI;
using CelesteEditor.BuildSystem;
using CelesteEditor.Tools;
using System;
using UnityEditor;
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
using CelesteEditor.BuildSystem.Data;
using CelesteEditor.Scene;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine.SceneManagement;
using Celeste.Sound;
using Celeste.Tools;
using CelesteEditor.BuildSystem.Steps;
using Celeste.Scene.Catalogue;
using Celeste.Log;
using TMPro;
using System.Reflection;


#if USE_ADDRESSABLES
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using CelesteEditor.Assets;
using CelesteEditor.Assets.Schemas;
using CelesteEditor.Assets.Analyze;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using CelesteEditor.Scene.Analyze;
#endif

namespace CelesteEditor.UnityProject
{
    [Serializable]
    public struct SetUpCelesteParameters
    {
        #region Properties and Fields

        private string FullyDelimitedPackagePath => packagePath.EndsWith('/') ? packagePath : packagePath + '/';

        public CelesteConstants CelesteConstants => new CelesteConstants(FullyDelimitedPackagePath);
        public BuildSystemConstants BuildSystemConstants => new BuildSystemConstants(FullyDelimitedPackagePath);

        public SceneType DefaultSceneType
        {
            get
            {
#if USE_ADDRESSABLES
                return usesAddressables ? SceneType.Addressable : SceneType.Baked;
#else
                return SceneType.Baked;
#endif
            }
        }

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
        [LabelWidth(300)] public bool useRuntimeInspectorPackage;
        [LabelWidth(300)] public bool useUnityAndroidLogcatPackage;
        [LabelWidth(300)] public bool removeUnityCollabPackage;
        [LabelWidth(300)] public List<string> dependenciesToAdd;
        [LabelWidth(300)] public List<string> dependenciesToRemove;

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
#if USE_ADDRESSABLES
        [LabelWidth(300)] public bool usesAddressables;
        [LabelWidth(300), ShowIf(nameof(usesAddressables))] public bool usesBakedGroupsWithRemoteOverride;
#endif
        [LabelWidth(300)] public bool usesTextMeshPro;

        [Header("Scenes")]
        [LabelWidth(300)] public bool needsStartupScene;
        [LabelWidth(300)] public bool needsBootstrapScene;
        [LabelWidth(300)] public bool needsEngineSystemsScene;
        [LabelWidth(300)] public bool needsLoadingScene;
        [LabelWidth(300)] public bool needsGameSystemsScene;

#endregion

        public void UseDefaults()
        {
            if (Directory.Exists(Path.Combine(Application.dataPath, "Celeste")))
            {
                packagePath = "Assets/Celeste/";
            }
            else if (Directory.Exists(Path.Combine(Application.dataPath, "CelesteUnity")))
            {
                packagePath = "Assets/Celeste";
            }
            else
            {
                packagePath = "Packages/com.celestegames.celeste/";
            }

            embedCeleste = false;

            usePresetGitIgnoreFile = true;
            usePresetGitLFSFile = false;

            useNativeSharePackage = true;
            useNativeFilePickerPackage = true;
            useRuntimeInspectorPackage = true;
            useUnityAndroidLogcatPackage = true;
            removeUnityCollabPackage = true;
            dependenciesToAdd = new List<string>();
            dependenciesToRemove = new List<string>();

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

            rootNamespaceName = Directory.GetParent(Application.dataPath).Name;
            rootMenuItemName = Directory.GetParent(Application.dataPath).Name;

#if USE_ADDRESSABLES
            usesAddressables = true;
            usesBakedGroupsWithRemoteOverride = true;
#endif
            usesTextMeshPro = true;

            needsStartupScene = true;
            needsBootstrapScene = true;
            needsEngineSystemsScene = true;
            needsLoadingScene = true;
            needsGameSystemsScene = true;
        }
    }

    public static class SetUpCeleste
    {
        #region Properties and Fields

        private const string IMPORT_TMPRO_ESSENTIALS_MENU_ITEM = "Window/TextMeshPro/Import TMP Essential Resources";

#if USE_ADDRESSABLES
        private static List<AnalyzeRule> addressableAnalyzeRules = new List<AnalyzeRule>()
        {
            new EnsureAssetsHaveNoOtherGroupLabelAnalyzeRule(),
            new EnsureAssetsHaveGroupLabelAnalyzeRule(),
            new EnsureBundledGroupsHaveRemoteBuildAndLoadPathsAnalyzeRule(),
            new EnsureGroupsHaveBundledSchemaAnalyzeRule(),
            new EnsureAddressableScenesSetUpCorrectlyAnalyzeRule()
        };
#endif

        #endregion

        public static void Execute(SetUpCelesteParameters parameters)
        {
            CreateAssetData(parameters);
            CreateEditorSettings(parameters);
            CreateProjectData(parameters);
            CreateBuildSystemData(parameters);
            CreateModules(parameters);
            CreateFileShareSettings();
            CreateCustomProguardFile(parameters);

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

#if USE_ADDRESSABLES
            if (parameters.usesAddressables)
            {
                foreach (AnalyzeRule analyzeRule in addressableAnalyzeRules)
                {
                    analyzeRule.FixIssues(AddressableAssetSettingsDefaultObject.GetSettings(false));
                }
            }
#endif
            
            CelesteSceneMenuItems.UpdateScenesInBuild();

            List<string> dependenciesToAdd = new List<string>(parameters.dependenciesToAdd);
            List<string> dependenciesToRemove = new List<string>(parameters.dependenciesToRemove);

            if (parameters.useNativeFilePickerPackage)
            {
                dependenciesToAdd.Add("git@github.com:AlanWills/UnityNativeFilePicker.git");
            }

            if (parameters.useNativeSharePackage)
            {
                dependenciesToAdd.Add("git@github.com:AlanWills/UnityNativeShare.git");
            }

            if (parameters.useRuntimeInspectorPackage)
            {
                dependenciesToAdd.Add("git@github.com:AlanWills/UnityRuntimeInspector.git");
            }

            if (parameters.useUnityAndroidLogcatPackage)
            {
                dependenciesToAdd.Add("com.unity.mobile.android-logcat");
            }

            if (parameters.removeUnityCollabPackage)
            {
                dependenciesToRemove.Add("com.unity.collab-proxy");
            }

            if (dependenciesToAdd.Count > 0)
            {
                Client.AddAndRemove(packagesToAdd: dependenciesToAdd.ToArray());
            }
        }


        private static void CreateModules(SetUpCelesteParameters parameters)
        {
            CreateLoading(parameters);
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

            // Addressable Group Names
#if USE_ADDRESSABLES
            {
                AddressableGroupNames addressableGroupNames = ScriptableObject.CreateInstance<AddressableGroupNames>();
                addressableGroupNames.name = nameof(AddressableGroupNames);
                addressableGroupNames.UseAllCreatedAddressableGroups = true;

                EditorOnly.CreateAssetInFolder(addressableGroupNames, BuildSystemConstants.EDITOR_FOLDER);
            }
#endif

            // Build Preparation Steps
            {
                DeleteBuildDirectory deleteBuildDirectory = ScriptableObject.CreateInstance<DeleteBuildDirectory>();
                deleteBuildDirectory.name = nameof(DeleteBuildDirectory);

                EditorOnly.CreateAssetInFolder(deleteBuildDirectory, BuildSystemConstants.BUILD_PREPARATION_STEPS_FOLDER);

                BuildPreparationSteps buildPreparationSteps = ScriptableObject.CreateInstance<BuildPreparationSteps>();
                buildPreparationSteps.name = nameof(BuildPreparationSteps);
                buildPreparationSteps.AddItem(deleteBuildDirectory);

                EditorOnly.CreateAssetInFolder(buildPreparationSteps, BuildSystemConstants.BUILD_PREPARATION_STEPS_FOLDER);
            }

            // Build Post Process Steps
            {
                WriteBuildEnvironmentVariablesToFile writeBuildEnvironmentVariablesToFile = ScriptableObject.CreateInstance<WriteBuildEnvironmentVariablesToFile>();
                writeBuildEnvironmentVariablesToFile.name = nameof(WriteBuildEnvironmentVariablesToFile);

                EditorOnly.CreateAssetInFolder(writeBuildEnvironmentVariablesToFile, BuildSystemConstants.BUILD_POST_PROCESS_STEPS_FOLDER);

                BuildPostProcessSteps buildPostProcessSteps = ScriptableObject.CreateInstance<BuildPostProcessSteps>();
                buildPostProcessSteps.name = nameof(BuildPostProcessSteps);
                buildPostProcessSteps.AddItem(writeBuildEnvironmentVariablesToFile);

                EditorOnly.CreateAssetInFolder(buildPostProcessSteps, BuildSystemConstants.BUILD_POST_PROCESS_STEPS_FOLDER);
            }

            // Asset Preparation Steps
            {
                DeleteAddressablesRemoteDirectory deleteAddressablesRemoteDirectory = ScriptableObject.CreateInstance<DeleteAddressablesRemoteDirectory>();
                deleteAddressablesRemoteDirectory.name = nameof(DeleteAddressablesRemoteDirectory);

                EditorOnly.CreateAssetInFolder(deleteAddressablesRemoteDirectory, BuildSystemConstants.ASSET_PREPARATION_STEPS_FOLDER);

                AssetPreparationSteps assetPreparationSteps = ScriptableObject.CreateInstance<AssetPreparationSteps>();
                assetPreparationSteps.name = nameof(AssetPreparationSteps);
                assetPreparationSteps.AddItem(deleteAddressablesRemoteDirectory);

                EditorOnly.CreateAssetInFolder(assetPreparationSteps, BuildSystemConstants.ASSET_PREPARATION_STEPS_FOLDER);
            }

            // Asset Post Process Steps
#if USE_ADDRESSABLES
            {
                BundleRemoteAddressablesInBuild bundleRemoteAddressablesInBuild = ScriptableObject.CreateInstance<BundleRemoteAddressablesInBuild>();
                bundleRemoteAddressablesInBuild.name = nameof(BundleRemoteAddressablesInBuild);

                EditorOnly.CreateAssetInFolder(bundleRemoteAddressablesInBuild, BuildSystemConstants.ASSET_POST_PROCESS_STEPS_FOLDER);

                WriteAssetEnvironmentVariablesToFile writeAssetEnvironmentVariablesToFile = ScriptableObject.CreateInstance<WriteAssetEnvironmentVariablesToFile>();
                writeAssetEnvironmentVariablesToFile.name = nameof(WriteAssetEnvironmentVariablesToFile);

                EditorOnly.CreateAssetInFolder(writeAssetEnvironmentVariablesToFile, BuildSystemConstants.ASSET_POST_PROCESS_STEPS_FOLDER);

                AssetPostProcessSteps assetPostProcessStepsForBuild = ScriptableObject.CreateInstance<AssetPostProcessSteps>();
                assetPostProcessStepsForBuild.name = $"{nameof(AssetPostProcessSteps)}ForBuild";
                assetPostProcessStepsForBuild.AddItem(bundleRemoteAddressablesInBuild);
                assetPostProcessStepsForBuild.AddItem(writeAssetEnvironmentVariablesToFile);

                EditorOnly.CreateAssetInFolder(assetPostProcessStepsForBuild, BuildSystemConstants.ASSET_POST_PROCESS_STEPS_FOLDER);

                AssetPostProcessSteps assetPostProcessStepsForUpdate = ScriptableObject.CreateInstance<AssetPostProcessSteps>();
                assetPostProcessStepsForUpdate.name = $"{nameof(AssetPostProcessSteps)}ForUpdate";
                assetPostProcessStepsForUpdate.AddItem(writeAssetEnvironmentVariablesToFile);

                EditorOnly.CreateAssetInFolder(assetPostProcessStepsForUpdate, BuildSystemConstants.ASSET_POST_PROCESS_STEPS_FOLDER);
            }
#endif

            // Create Scripting Define Symbols
            EditorOnly.CreateFolder(BuildSystemConstants.SCRIPTING_DEFINE_SYMBOLS_FOLDER);

            // Debug
            {
                ScriptingDefineSymbols debugScriptingDefineSymbols = ScriptableObject.CreateInstance<ScriptingDefineSymbols>();
                debugScriptingDefineSymbols.name = $"Debug{nameof(ScriptingDefineSymbols)}";
                debugScriptingDefineSymbols.AddDefaultDebugSymbols();
                EditorOnly.CreateAssetInFolder(debugScriptingDefineSymbols, BuildSystemConstants.SCRIPTING_DEFINE_SYMBOLS_FOLDER);
            }

            // Release
            {
                ScriptingDefineSymbols debugScriptingDefineSymbols = ScriptableObject.CreateInstance<ScriptingDefineSymbols>();
                debugScriptingDefineSymbols.name = $"Release{nameof(ScriptingDefineSymbols)}";
                debugScriptingDefineSymbols.AddDefaultReleaseSymbols();
                EditorOnly.CreateAssetInFolder(debugScriptingDefineSymbols, BuildSystemConstants.SCRIPTING_DEFINE_SYMBOLS_FOLDER);
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
                EditorOnly.CreateFolder(BuildSystemConstants.FOLDER_PATH);

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
        }

#endregion

        #region Startup

        private static void CreateStartup(SetUpCelesteParameters parameters)
        {
            if (parameters.needsStartupScene)
            {
                CreateStartupFolders();
                CreateStartupLoadJob(parameters);
                CreateStartupScene(parameters);
                CreateStartupAssemblies(parameters);
            }
        }

        private static void CreateStartupFolders()
        {
            EditorOnly.CreateFolder(StartupConstants.SCENES_FOLDER_PATH);
            EditorOnly.CreateFolder(StartupConstants.LOAD_JOBS_FOLDER_PATH);
        }

        private static void CreateStartupLoadJob(SetUpCelesteParameters parameters)
        {
            var startupLoadJobBuilder = new MultiLoadJob.Builder()
                .WithShowOutputInLoadingScreen(false);

#if USE_ADDRESSABLES
            if (parameters.usesAddressables && parameters.usesBakedGroupsWithRemoteOverride)
            {
                // Load content catalogue load job
                {
                    LoadContentCatalogueLoadJob loadContentCatalogue = ScriptableObject.CreateInstance<LoadContentCatalogueLoadJob>();
                    loadContentCatalogue.name = StartupConstants.LOAD_CONTENT_CATALOGUE_LOAD_JOB_NAME;
                    startupLoadJobBuilder.WithLoadJob(loadContentCatalogue);

                    EditorOnly.CreateAssetInFolder(loadContentCatalogue, StartupConstants.LOAD_JOBS_FOLDER_PATH);
                }

                // Enable bundled addressables load job
                {
                    EnableBundledAddressablesLoadJob enableBundledAddressables = ScriptableObject.CreateInstance<EnableBundledAddressablesLoadJob>();
                    enableBundledAddressables.name = StartupConstants.ENABLE_BUNDLED_ADDRESSABLES_LOAD_JOB_NAME;
                    startupLoadJobBuilder.WithLoadJob(enableBundledAddressables);

                    EditorOnly.CreateAssetInFolder(enableBundledAddressables, StartupConstants.LOAD_JOBS_FOLDER_PATH);
                }

                // Download Bootstrap addressables load job
                {
                    DownloadAddressablesLoadJob downloadAddressablesLoadJob = ScriptableObject.CreateInstance<DownloadAddressablesLoadJob>();
                    downloadAddressablesLoadJob.name = StartupConstants.DOWNLOAD_BOOTSTRAP_ADDRESSABLES_LOAD_JOB_NAME;
                    downloadAddressablesLoadJob.AddressablesLabel = BootstrapConstants.ADDRESSABLES_GROUP_NAME;
                    startupLoadJobBuilder.WithLoadJob(downloadAddressablesLoadJob);

                    EditorOnly.CreateAssetInFolder(downloadAddressablesLoadJob, StartupConstants.LOAD_JOBS_FOLDER_PATH);
                }
            }
#endif

            // Load bootstrap scene set load job
            {
                SceneSet bootstrapSceneSet = EditorOnly.FindAsset<SceneSet>(BootstrapConstants.SCENE_SET_NAME);
                Debug.Assert(bootstrapSceneSet != null, $"Could not find bootstrap scene set for load job: {BootstrapConstants.SCENE_SET_NAME}.  It will have to be set manually later, after the scene set is created.");
                var loadBootstrapSceneSetBuilder = new LoadSceneSetLoadJob.Builder()
                    .WithLoadSceneMode(LoadSceneMode.Single)
                    .WithSceneSet(bootstrapSceneSet)
                    .WithShowOutputOnLoadingScreen(false);

                LoadSceneSetLoadJob loadBootstrapSceneSet = loadBootstrapSceneSetBuilder.Build();
                loadBootstrapSceneSet.name = StartupConstants.LOAD_BOOTSTRAP_SCENE_SET_LOAD_JOB_NAME;
                startupLoadJobBuilder.WithLoadJob(loadBootstrapSceneSet);

                EditorOnly.CreateAssetInFolder(loadBootstrapSceneSet, StartupConstants.LOAD_JOBS_FOLDER_PATH);
            }

            LoadJob startupLoadJob = startupLoadJobBuilder.Build();
            startupLoadJob.name = StartupConstants.LOAD_JOB_NAME;
            EditorOnly.CreateAssetInFolder(startupLoadJob, StartupConstants.LOAD_JOBS_FOLDER_PATH);
        }

        private static void CreateStartupScene(SetUpCelesteParameters parameters)
        {
            UnityEngine.SceneManagement.Scene startupScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Create main camera
            {
                GameObject cameraGameObject = new GameObject("Main Camera");
                cameraGameObject.tag = "MainCamera";
                Camera camera = cameraGameObject.AddComponent<Camera>();
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = Color.white;
                camera.orthographic = true;
            }

            // Create startup load component
            {
                GameObject startupLoadGameObject = new GameObject(nameof(StartupLoad));
                StartupLoad startupLoad = startupLoadGameObject.AddComponent<StartupLoad>();
                LoadJob startupLoadJob = EditorOnly.FindAsset<LoadJob>(StartupConstants.LOAD_JOB_NAME);
                Debug.Assert(startupLoadJob != null, $"Could not find startup load job: {StartupConstants.LOAD_JOB_NAME}.  It will have to be set after it is created.");
                startupLoad.StartupLoadJob = startupLoadJob;
            }

            EditorSceneManager.SaveScene(startupScene, StartupConstants.SCENE_PATH);

            SceneSet sceneSet = ScriptableObject.CreateInstance<SceneSet>();
            sceneSet.name = StartupConstants.SCENE_SET_NAME;
            sceneSet.MenuItemPath = $"{parameters.rootMenuItemName}/Scenes/Load {StartupConstants.SCENE_NAME}";
            sceneSet.AddScene(StartupConstants.SCENE_NAME, SceneType.Baked, false);
            EditorOnly.CreateAssetInFolder(sceneSet, StartupConstants.SCENES_FOLDER_PATH);

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
            EditorOnly.CreateFolder(BootstrapConstants.SCENES_FOLDER_PATH);
            EditorOnly.CreateFolder(BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
        }

        private static void CreateBootstrapLoadJob(SetUpCelesteParameters parameters)
        {
            var bootstrapLoadJobBuilder = new MultiLoadJob.Builder()
                .WithShowOutputInLoadingScreen(false);

            // Disable fallback assets load job
            {
                var disableFallbackAssets = EditorOnly.FindAsset<LoadJob>(CelesteConstants.DISABLE_FALLBACK_LOAD_ASSETS_LOAD_JOB_NAME);
                Debug.Assert(disableFallbackAssets != null, $"Could not find disable fallback load assets load job: {CelesteConstants.DISABLE_FALLBACK_LOAD_ASSETS_LOAD_JOB_NAME}.");
                bootstrapLoadJobBuilder.WithLoadJob(disableFallbackAssets);
                MakeAddressable(parameters, disableFallbackAssets, BootstrapConstants.ADDRESSABLES_GROUP_NAME);
            }

#if USE_ADDRESSABLES
            // Download Bootstrap Addressables load job
            {
                DownloadAddressablesLoadJob downloadCommonAddressablesLoadJob = ScriptableObject.CreateInstance<DownloadAddressablesLoadJob>();
                downloadCommonAddressablesLoadJob.name = BootstrapConstants.DOWNLOAD_COMMON_ADDRESSABLES_LOAD_JOB_NAME;
                downloadCommonAddressablesLoadJob.AddressablesLabel = CelesteConstants.COMMON_ADDRESSABLES_GROUP_NAME;
                bootstrapLoadJobBuilder.WithLoadJob(downloadCommonAddressablesLoadJob);

                EditorOnly.CreateAssetInFolder(downloadCommonAddressablesLoadJob, BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
                MakeAddressable(parameters, downloadCommonAddressablesLoadJob, BootstrapConstants.ADDRESSABLES_GROUP_NAME);
            }
#endif

            // Load engine systems scene set load job
            {
                SceneSet engineSystemsSceneSet = EditorOnly.FindAsset<SceneSet>(EngineSystemsConstants.SCENE_SET_NAME);
                Debug.Assert(engineSystemsSceneSet != null, $"Could not find engine systems scene set for load job: {EngineSystemsConstants.SCENE_SET_NAME}.  It will have to be set manually later, after the scene set is created.");
                var loadEngineSystemsSceneSetBuilder = new LoadSceneSetLoadJob.Builder()
                    .WithLoadSceneMode(LoadSceneMode.Additive)
                    .WithSceneSet(engineSystemsSceneSet)
                    .WithShowOutputOnLoadingScreen(false);

                LoadSceneSetLoadJob loadEngineSystemsSceneSet = loadEngineSystemsSceneSetBuilder.Build();
                loadEngineSystemsSceneSet.name = BootstrapConstants.LOAD_ENGINE_SYSTEMS_SCENE_SET_LOAD_JOB_NAME;
                bootstrapLoadJobBuilder.WithLoadJob(loadEngineSystemsSceneSet);

                EditorOnly.CreateAssetInFolder(loadEngineSystemsSceneSet, BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
                MakeAddressable(parameters, loadEngineSystemsSceneSet, BootstrapConstants.ADDRESSABLES_GROUP_NAME);
            }

            // Load game systems scene set load job
            {
                SceneSet gameSystemsSceneSet = EditorOnly.FindAsset<SceneSet>(GameSystemsConstants.SCENE_SET_NAME);
                Debug.Assert(gameSystemsSceneSet != null, $"Could not find game systems scene set for load job: {GameSystemsConstants.SCENE_SET_NAME}.  It will have to be set manually later, after the scene set is created.");
                var loadGameSystemsSceneSetBuilder = new LoadSceneSetLoadJob.Builder()
                    .WithLoadSceneMode(LoadSceneMode.Additive)
                    .WithSceneSet(gameSystemsSceneSet)
                    .WithShowOutputOnLoadingScreen(false);

                LoadSceneSetLoadJob loadGameSystemsSceneSet = loadGameSystemsSceneSetBuilder.Build();
                loadGameSystemsSceneSet.name = BootstrapConstants.LOAD_GAME_SYSTEMS_SCENE_SET_LOAD_JOB_NAME;
                bootstrapLoadJobBuilder.WithLoadJob(loadGameSystemsSceneSet);

                EditorOnly.CreateAssetInFolder(loadGameSystemsSceneSet, BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
                MakeAddressable(parameters, loadGameSystemsSceneSet, BootstrapConstants.ADDRESSABLES_GROUP_NAME);
            }

            LoadJob bootstrapLoadJob = bootstrapLoadJobBuilder.Build();
            bootstrapLoadJob.name = BootstrapConstants.LOAD_JOB_NAME;

            EditorOnly.CreateAssetInFolder(bootstrapLoadJob, BootstrapConstants.LOAD_JOBS_FOLDER_PATH);
            MakeAddressable(parameters, bootstrapLoadJob, BootstrapConstants.ADDRESSABLES_GROUP_NAME);
        }

        private static void CreateBootstrapScene(SetUpCelesteParameters parameters)
        {
            UnityEngine.SceneManagement.Scene bootstrapScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Create main camera
            {
                GameObject cameraGameObject = new GameObject("Main Camera");
                cameraGameObject.tag = "MainCamera";
                Camera camera = cameraGameObject.AddComponent<Camera>();
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = Color.white;
                camera.orthographic = true;
            }

            GameObject bootstrapManagerPrefab = EditorOnly.FindAsset<GameObject>(BootstrapConstants.BOOTSTRAP_MANAGER_PREFAB_NAME);
            Debug.Assert(bootstrapManagerPrefab != null, $"Could not find bootstrap manager prefab: {BootstrapConstants.BOOTSTRAP_MANAGER_PREFAB_NAME}.");

            GameObject bootstrapManagerInstance = PrefabUtility.InstantiatePrefab(bootstrapManagerPrefab, bootstrapScene) as GameObject;
            LoadJob bootstrapLoadJob = EditorOnly.FindAsset<LoadJob>(BootstrapConstants.LOAD_JOB_NAME);
            Debug.Assert(bootstrapLoadJob != null, $"Could not find bootstrap load job: {BootstrapConstants.LOAD_JOB_NAME}.  It will have to be set manually after it is created.");
            bootstrapManagerInstance.GetComponent<BootstrapManager>().bootstrapJob = bootstrapLoadJob;
            EditorUtility.SetDirty(bootstrapManagerInstance);
            EditorSceneManager.SaveScene(bootstrapScene, BootstrapConstants.SCENE_PATH);
            AssetDatabase.LoadAssetAtPath<SceneAsset>(parameters.CelesteConstants.LOADING_SCENE_PATH).SetAddressableInfo(BootstrapConstants.ADDRESSABLES_GROUP_NAME, LoadingConstants.SCENE_NAME);
            AssetDatabase.LoadAssetAtPath<SceneAsset>(BootstrapConstants.SCENE_PATH).SetAddressableInfo(BootstrapConstants.ADDRESSABLES_GROUP_NAME, BootstrapConstants.SCENE_NAME);

            SceneSet bootstrapSceneSet = ScriptableObject.CreateInstance<SceneSet>();
            bootstrapSceneSet.name = BootstrapConstants.SCENE_SET_NAME;
            bootstrapSceneSet.MenuItemPath = $"{parameters.rootMenuItemName}/Scenes/Load {BootstrapConstants.SCENE_NAME}";
            bootstrapSceneSet.AddScene(LoadingConstants.SCENE_NAME, parameters.DefaultSceneType, false); // This must be first
            bootstrapSceneSet.AddScene(BootstrapConstants.SCENE_NAME, parameters.DefaultSceneType, false);
            bootstrapSceneSet.HasCustomDebugBuildValue = false;

            EditorOnly.CreateAssetInFolder(bootstrapSceneSet, BootstrapConstants.SCENES_FOLDER_PATH);
        }

        private static void CreateBootstrapAssemblies(SetUpCelesteParameters parameters)
        {
            CreateAssembliesParameters bootstrapAssembly = new CreateAssembliesParameters();
            bootstrapAssembly.hasEditorAssembly = true;
            bootstrapAssembly.assemblyName = $"{parameters.rootNamespaceName}.{BootstrapConstants.NAMESPACE_NAME}";
            bootstrapAssembly.directoryName = BootstrapConstants.FOLDER_NAME;

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
            EditorOnly.CreateFolder(EngineSystemsConstants.SCENES_FOLDER_PATH);
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
            engineSystemsSceneSet.MenuItemPath = $"{parameters.rootMenuItemName}/Scenes/Load {EngineSystemsConstants.SCENE_NAME}";
            engineSystemsSceneSet.AddScene(LoadingConstants.SCENE_NAME, parameters.DefaultSceneType, false);
            engineSystemsSceneSet.AddScene(EngineSystemsConstants.SCENE_NAME, parameters.DefaultSceneType, false);
            engineSystemsSceneSet.AddScene(EngineSystemsConstants.DEBUG_SCENE_NAME, parameters.DefaultSceneType, true);

            EditorOnly.CreateAssetInFolder(engineSystemsSceneSet, EngineSystemsConstants.SCENES_FOLDER_PATH);
            MakeAddressable(parameters, engineSystemsSceneSet, BootstrapConstants.ADDRESSABLES_GROUP_NAME);
        }

        private static void CreateEngineSystemsAssets(SetUpCelesteParameters parameters)
        {
            MusicSettings musicSettings = ScriptableObject.CreateInstance<MusicSettingsUsingAssets>();
            musicSettings.name = nameof(MusicSettings);

            SFXSettingsUsingAssets sfxSettings = ScriptableObject.CreateInstance<SFXSettingsUsingAssets>();
            sfxSettings.name = nameof(SFXSettings);

            SectionLogSettingsCatalogue sectionLogSettingsCatalogue = ScriptableObject.CreateInstance<SectionLogSettingsCatalogue>();
            sectionLogSettingsCatalogue.name = nameof(SectionLogSettingsCatalogue);

            EditorOnly.CreateAssetInFolder(musicSettings, EngineSystemsConstants.DATA_FOLDER_PATH);
            EditorOnly.CreateAssetInFolder(sfxSettings, EngineSystemsConstants.DATA_FOLDER_PATH);
            EditorOnly.CreateAssetInFolder(sectionLogSettingsCatalogue, EngineSystemsConstants.DATA_FOLDER_PATH);

            MakeAddressable(parameters, musicSettings);
            MakeAddressable(parameters, sfxSettings);
            MakeAddressable(parameters, sectionLogSettingsCatalogue);

            UnityEngine.SceneManagement.Scene engineSystemsScene = EditorSceneManager.OpenScene(EngineSystemsConstants.SCENE_PATH, OpenSceneMode.Single);

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

            // Set Section Log Settings Catalogue
            {
                LogManager logManager = GameObject.FindFirstObjectByType<LogManager>();
                Debug.Assert(logManager, $"Could not find {nameof(LogManager)} in the Engine Systems scene!  You'll have to assign Log settings manually...");

                if (logManager != null)
                {
                    logManager.SectionLogSettingsCatalogue = sectionLogSettingsCatalogue;
                }
            }

            EditorSceneManager.SaveScene(engineSystemsScene, EngineSystemsConstants.SCENE_PATH);
        }

        #endregion

        #region Loading

        private static void CreateLoading(SetUpCelesteParameters parameters)
        {
            if (parameters.needsLoadingScene)
            {
                CreateLoadingFolders();
                CreateLoadingScenes(parameters);
            }
        }

        private static void CreateLoadingFolders()
        {
            EditorOnly.CreateFolder(LoadingConstants.SCENES_FOLDER_PATH);
        }

        private static void CreateLoadingScenes(SetUpCelesteParameters parameters)
        {
            bool copySuccessful = AssetDatabase.CopyAsset(parameters.CelesteConstants.LOADING_SCENE_PATH, LoadingConstants.SCENES_FOLDER_PATH);
            Debug.Assert(copySuccessful, $"Failed to copy Celeste Loading scene from '{parameters.CelesteConstants.LOADING_SCENE_PATH}' to '{LoadingConstants.SCENES_FOLDER_PATH}'.");

            AssetDatabase.Refresh();
            SetAddressableAddress(parameters, LoadingConstants.SCENES_FOLDER_PATH, LoadingConstants.SCENE_NAME);
        }

        #endregion

        #region Game Systems

        private static void CreateGameSystems(SetUpCelesteParameters parameters)
        {
            if (parameters.needsGameSystemsScene)
            {
                CreateGameSystemsFolders();
                CreateGameSystemsScenes(parameters);
                CreateGameSystemsAssets(parameters);
            }
        }

        private static void CreateGameSystemsFolders()
        {
            EditorOnly.CreateFolder(GameSystemsConstants.SCENES_FOLDER_PATH);
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
            gameSystemsSceneSet.MenuItemPath = $"{parameters.rootMenuItemName}/Scenes/Load {GameSystemsConstants.SCENE_NAME}";
            gameSystemsSceneSet.AddScene(GameSystemsConstants.SCENE_NAME, parameters.DefaultSceneType, false);
            gameSystemsSceneSet.AddScene(GameSystemsConstants.DEBUG_SCENE_NAME, parameters.DefaultSceneType, true);

            EditorOnly.CreateAssetInFolder(gameSystemsSceneSet, GameSystemsConstants.SCENES_FOLDER_PATH);
            MakeAddressable(parameters, gameSystemsSceneSet, BootstrapConstants.ADDRESSABLES_GROUP_NAME);
        }

        private static void CreateGameSystemsAssets(SetUpCelesteParameters parameters)
        {
            SceneSetCatalogue sceneSetCatalogue = ScriptableObject.CreateInstance<SceneSetCatalogue>();
            sceneSetCatalogue.name = nameof(SceneSetCatalogue);

            EditorOnly.CreateAssetInFolder(sceneSetCatalogue, GameSystemsConstants.DATA_FOLDER_PATH);
            MakeAddressable(parameters, sceneSetCatalogue);
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

        #region Custom Proguard File

        private static void CreateCustomProguardFile(SetUpCelesteParameters parameters)
        {
            if (parameters.useNativeFilePickerPackage ||
                parameters.useNativeSharePackage ||
                parameters.useRuntimeInspectorPackage)
            {
                string proguardFilePath = Path.Combine(Application.dataPath, "Plugins", "Android", "proguard-user.txt");
                File.WriteAllText(proguardFilePath, CelesteConstants.CUSTOM_PROGUARD_FILE_CONTENTS);
            }
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
                EditorOnly.CreateFolder(newDirectory);

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
            GameObject prefab = EditorOnly.FindAsset<GameObject>(scenePrefabName);
            PrefabUtility.InstantiatePrefab(prefab, scene);
            EditorSceneManager.SaveScene(scene, scenePath);
            AssetDatabase.Refresh();

            SetAddressableAddress(parameters,scenePath, sceneName);
        }

        private static void CreateAssetData(SetUpCelesteParameters parameters)
        {
#if USE_ADDRESSABLES
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
                settings.MonoScriptBundleNaming = UnityEditor.AddressableAssets.Build.MonoScriptBundleNaming.Custom;
                settings.MonoScriptBundleCustomNaming = "alwaysbundle";

                {
                    bool loadPathResult = settings.RemoteCatalogLoadPath.SetVariableByName(settings, "Remote.LoadPath");
                    Debug.Assert(loadPathResult, "Failed to set Remote Catalog Load Path to 'Remote'.  This will need to be done manually in the Addressable Settings.");
                }

                {
                    bool buildPathResult = settings.RemoteCatalogBuildPath.SetVariableByName(settings, "Remote.BuildPath");
                    Debug.Assert(buildPathResult, "Failed to set Remote Catalog Build Path to 'Remote'.  This will need to be done manually in the Addressable Settings.");
                }

                string remoteBuildPath = AddressablesExtensions.GetAddressablesRemoteBuildPath();
                string remoteLoadPath = AddressablesExtensions.GetAddressablesRemoteLoadPath();

                // Rename default group to and add tag for Common Addressables
                {
                    settings.AddLabel(CelesteConstants.COMMON_ADDRESSABLES_GROUP_NAME, false);
                    settings.DefaultGroup.Name = CelesteConstants.COMMON_ADDRESSABLES_GROUP_NAME;
                    settings.DefaultGroup.SetBuildPath(remoteBuildPath);
                    settings.DefaultGroup.SetLoadPath(remoteLoadPath);

                    if (parameters.usesBakedGroupsWithRemoteOverride)
                    {
                        settings.DefaultGroup.AddSchema<BundledGroupSchema>(false);
                    }
                }

                // Add Bootstrap addressables group for initial assets
                {
                    settings.AddLabel(BootstrapConstants.ADDRESSABLES_GROUP_NAME, false);

                    AddressableAssetGroup bootstrapAddressables = settings.CreateGroup(BootstrapConstants.ADDRESSABLES_GROUP_NAME, false, false, false, new List<AddressableAssetGroupSchema>());
                    bootstrapAddressables.AddSchema<BundledAssetGroupSchema>(false);
                    bootstrapAddressables.AddSchema<ContentUpdateGroupSchema>();
                    bootstrapAddressables.SetBuildPath(remoteBuildPath);
                    bootstrapAddressables.SetLoadPath(remoteLoadPath);

                    if (parameters.usesBakedGroupsWithRemoteOverride)
                    {
                        bootstrapAddressables.AddSchema<BundledGroupSchema>(false);
                    }
                }

                settings.RemoveLabel("default");
            }
#endif

            if (parameters.usesTextMeshPro)
            {
                string packageFullPath = TMP_EditorUtility.packageFullPath;
                AssetDatabase.ImportPackage($"{packageFullPath}/Package Resources/TMP Essential Resources.unitypackage", false);

                TMP_Settings tmpSettings = TMP_Settings.LoadDefaultSettings();
                Debug.Assert(tmpSettings, "Failed to load Text Mesh Pro default settings, so we will be unable to turn off raycasting by default on Text Mesh Pro components.");

                if (tmpSettings != null)
                {
                    // Turn off raycast by default
                    FieldInfo fieldInfo = typeof(TMP_Settings).GetField("m_EnableRaycastTarget");
                    Debug.Assert(tmpSettings, "Failed find the raycast setting on the Text Mesh Pro default settings, so we will be unable to turn off raycasting by default on Text Mesh Pro components.");

                    if (fieldInfo != null)
                    {
                        fieldInfo.SetValue(tmpSettings, false);
                        EditorOnly.SaveAsset(tmpSettings);
                    }
                }
            }
        }

        private static void MakeAddressable(SetUpCelesteParameters parameters, UnityEngine.Object obj)
        {
#if USE_ADDRESSABLES
            if (parameters.usesAddressables)
            {
                obj.MakeAddressable();
            }
#endif
        }

        private static void MakeAddressable(SetUpCelesteParameters parameters, UnityEngine.Object obj, string groupName)
        {
#if USE_ADDRESSABLES
            if (parameters.usesAddressables)
            {
                obj.MakeAddressable(groupName);
            }
#endif
        }

        private static void SetAddressableAddress(SetUpCelesteParameters parameters, UnityEngine.Object obj, string address)
        {
#if USE_ADDRESSABLES
            if (parameters.usesAddressables)
            {
                obj.SetAddressableAddress(address);
            }
#endif
        }

        private static void SetAddressableAddress(SetUpCelesteParameters parameters, string assetPath, string address)
        {
#if USE_ADDRESSABLES
            if (parameters.usesAddressables)
            {
                AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath).SetAddressableAddress(address);
            }
#endif
        }

        #endregion
    }
}
