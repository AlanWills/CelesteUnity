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
        [LabelWidth(300)] public bool usePresetGitIgnoreFile;
        [LabelWidth(300)] public bool usePresetGitLFSFile;

        [Header("Build System")]
        [LabelWidth(300)] public bool needsBuildSystem;
        [LabelWidth(300), ShowIf(nameof(needsBuildSystem))] public bool runsOnWindows;
        [LabelWidth(300), ShowIf(nameof(needsBuildSystem))] public bool runsOnAndroid;
        [LabelWidth(300), ShowIf(nameof(needsBuildSystem))] public bool runsOniOS;
        [LabelWidth(300), ShowIf(nameof(needsBuildSystem))] public bool runsOnWebGL;
        [LabelWidth(300), ShowIf(nameof(needsBuildSystem))]
        [Tooltip("If true, copies of Common template jenkins files will be added to the project for customisation and usage")]
        public bool useCommonJenkinsFiles;
        [LabelWidth(300), ShowIf(nameof(runsOnWindows))]
        [Tooltip("If true, copies of Windows template jenkins files will be added to the project for customisation and usage")]
        public bool useWindowsBuildJenkinsFiles;
        [LabelWidth(300), ShowIf(nameof(runsOnAndroid))]
        [Tooltip("If true, copies of the Android template jenkins files will be added to the project for customisation and usage")]
        public bool useAndroidBuildJenkinsFiles;
        [LabelWidth(300), ShowIf(nameof(runsOniOS))]
        [Tooltip("If true, copies of the iOS template jenkins files will be added to the project for customisation and usage")]
        public bool useiOSBuildJenkinsFiles;
        [LabelWidth(300), ShowIf(nameof(runsOnWebGL))]
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

        #endregion

        public void UseDefaults()
        {
            packagePath = "Packages/com.celestegames.celeste/";
            usePresetGitIgnoreFile = true;
            usePresetGitLFSFile = true;

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
            CreateEditorSettings();
            CreateFileShareSettings();
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
                CreateBootstrapScene(parameters);
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
                    .WithLoadSceneMode(UnityEngine.SceneManagement.LoadSceneMode.Additive)
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
            bootstrapSceneSet.AddScene(CelesteConstants.LOADING_SCENE_NAME, SceneType.Addressable, false); // This must be first
            bootstrapSceneSet.AddScene(BootstrapConstants.SCENE_NAME, SceneType.Addressable, false);
            
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
            engineSystemsSceneSet.AddScene(EngineSystemsConstants.SCENE_NAME, SceneType.Addressable, false);
            
            AssetUtility.CreateAssetInFolder(engineSystemsSceneSet, EngineSystemsConstants.SCENES_FOLDER_PATH);
            engineSystemsSceneSet.MakeAddressable();
        }

        #endregion

        #region Editor Settings

        private static void CreateEditorSettings()
        {
            DataImporterEditorSettings.GetOrCreateSettings();
            DebugEditorSettings.GetOrCreateSettings();
            LiveOpsEditorSettings.GetOrCreateSettings();
            LocalisationEditorSettings.GetOrCreateSettings();
            PersistenceEditorSettings.GetOrCreateSettings();
            SceneEditorSettings.GetOrCreateSettings();
            SoundEditorSettings.GetOrCreateSettings();
            InputEditorSettings.GetOrCreateSettings();
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
            if (Directory.Exists(originalDirectory))
            {
                DirectoryInfo originalDirectoryInfo = new DirectoryInfo(originalDirectory);
                AssetUtility.CreateFolder(newDirectory);

                foreach (FileInfo file in originalDirectoryInfo.GetFiles(originalDirectory))
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

        private static void CreateModules(SetUpCelesteParameters parameters)
        {
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
