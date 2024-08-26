using Celeste.Constants;
using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
#if USE_ADDRESSABLES
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
#endif
using UnityEditor.Build.Reporting;
using UnityEngine;
using CelesteEditor.Tools;
using Celeste.Tools.Attributes.GUI;
using CelesteEditor.BuildSystem.Steps;
using CelesteEditor.BuildSystem.Data;
using CelesteEditor.Persistence;
using Celeste.Tools;

namespace CelesteEditor.BuildSystem
{
    public abstract class PlatformSettings : ScriptableObject
    {
        #region Properties and Fields

        private const string STRING_SUBSTITUTION_HELP = 
            "\n\n{version} will be replaced with the full version number." +
            "\n\n{major}, {minor} and {build} will be replaced with the corresponding values from the version." +
            "\n\n{build_target} will be replaced with the value of the 'BuildTarget' variable." +
            "\n\n{build_target_group} will be replaced with the value of the 'BuildTargetGroup' variable." +
            "\n\n{environment} will be replaced with 'Debug' or 'Release' if the 'IsDebugBuild' variable is true or false respectively.";

        [SerializeField]
        [Tooltip("The version number that corresponds to the Application.version string.  Usually of the form 'Major.Minor.Patch'.")]
        private AppVersion version;
        public AppVersion Version
        {
            get => version;
            set
            {
                if (version != value)
                {
                    version = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField]
        [Tooltip("The directory relative to the project directory that the build will be created in." + STRING_SUBSTITUTION_HELP)]
        private string buildDirectory;
        public string BuildDirectory
        {
            get => Resolve(buildDirectory);
            protected set
            {
                if (string.CompareOrdinal(buildDirectory, value) != 0)
                {
                    buildDirectory = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField]
        [Tooltip("The name of the outputted build." + STRING_SUBSTITUTION_HELP)]
        private string outputName;
        public string OutputName
        {
            get => Resolve(outputName);
            protected set
            {
                if (string.CompareOrdinal(outputName, value) != 0)
                {
                    outputName = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField]
        [Tooltip("The url path in the storage bucket you wish to upload the build too." + STRING_SUBSTITUTION_HELP)]
        private string buildUploadUrl;
        public string BuildUploadURL
        {
            get => Resolve(buildUploadUrl);
            protected set
            {
                if (string.CompareOrdinal(buildUploadUrl, value) != 0)
                {
                    buildUploadUrl = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField]
        [Tooltip("When making a build as part of a build pipeline, this value will be exported as an environment variable to allow you to provide credentials to whatever system you use for uploading builds.")]
        private string buildUploadCredentials;
        public string BuildUploadCredentials
        {
            get => buildUploadCredentials;
            protected set
            {
                if (string.CompareOrdinal(buildUploadCredentials, value) != 0)
                {
                    buildUploadCredentials = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

#if USE_ADDRESSABLES
        [SerializeField]
        [Tooltip("If enabled, addressable specific pipelines will run in the build pipeline.")]
        private bool addressablesEnabled = false;
        public bool AddressablesEnabled
        {
            get => addressablesEnabled;
            protected set
            {
                if (addressablesEnabled != value)
                {
                    addressablesEnabled = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("A custom string that will be appended to the end of the remote addressables catalogue name in place of the Unity-generated hash." + STRING_SUBSTITUTION_HELP)]
        private string playerOverrideVersion;
        public string PlayerOverrideVersion
        {
            get => Resolve(playerOverrideVersion);
            protected set
            {
                playerOverrideVersion = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("The directory that built addressables will be outputted to, relative to the project directory." + STRING_SUBSTITUTION_HELP)]
        private string addressablesBuildDirectory;
        public string AddressablesBuildDirectory
        {
            get => Resolve(addressablesBuildDirectory);
            set
            {
                if (string.CompareOrdinal(addressablesBuildDirectory, value) != 0)
                {
                    addressablesBuildDirectory = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("The remote load path for the addressables e.g. an S3 bucket." + STRING_SUBSTITUTION_HELP)]
        private string addressablesLoadDirectory;
        public string AddressablesLoadDirectory
        {
            get => Resolve(addressablesLoadDirectory);
            protected set
            {
                if (string.CompareOrdinal(addressablesLoadDirectory, value) != 0)
                {
                    addressablesLoadDirectory = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("When building addressables as part of a build pipeline, this value will be added to a file to allow uploading to a specific storage path within a bucket." + STRING_SUBSTITUTION_HELP)]
        private string addressablesUploadURL;
        public string AddressablesUploadURL
        {
            get => Resolve(addressablesUploadURL);
            protected set
            {
                if (string.CompareOrdinal(addressablesUploadURL, value) != 0)
                {
                    addressablesUploadURL = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("When building addressables as part of a build pipeline, this value will be exported as an environment variable to allow you to provide credentials to whatever system you use for uploading assets." + STRING_SUBSTITUTION_HELP)]
        private string addressablesUploadCredentials;
        public string AddressablesUploadCredentials
        {
            get => Resolve(addressablesUploadCredentials);
            protected set
            {
                if (string.CompareOrdinal(addressablesUploadCredentials, value) != 0)
                {
                    addressablesUploadCredentials = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("Enable advanced addressables settings.  Warning, most users will not need to edit these values.")]
        private bool advancedAddressablesSettings;

        [SerializeField, ShowIfAll(nameof(addressablesEnabled), nameof(advancedAddressablesSettings))]
        [Indent, Tooltip("The build path for local addressables that Unity uses to copy built addressable groups to Streaming Assets when building a player." + STRING_SUBSTITUTION_HELP)]
        private string localAddressablesBuildPath = "[UnityEngine.AddressableAssets.Addressables.BuildPath]/[BuildTarget]";
        public string LocalAddressablesBuildPath => Resolve(localAddressablesBuildPath);

        [SerializeField, ShowIfAll(nameof(addressablesEnabled), nameof(advancedAddressablesSettings))]
        [Indent, Tooltip("The load path that Unity uses at runtime to load local addressable groups it has bundled with the app." + STRING_SUBSTITUTION_HELP)]
        private string localAddressablesLoadPath = "{UnityEngine.AddressableAssets.Addressables.RuntimePath}/[BuildTarget]";
        public string LocalAddressablesLoadPath => Resolve(localAddressablesLoadPath);
#endif

        [SerializeField]
        private BuildTarget buildTarget;
        public BuildTarget BuildTarget
        {
            get => buildTarget;
            protected set
            {
                buildTarget = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        private BuildTargetGroup buildTargetGroup;
        public BuildTargetGroup BuildTargetGroup
        {
            get => buildTargetGroup;
            protected set
            {
                buildTargetGroup = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        [Tooltip("An internal unity development flag.")]
        private bool development = true;

        [SerializeField]
        [Tooltip("Controls the value of the Celeste 'IsDebugBuild' runtime flag.")]
        private bool isDebugBuild = false;

        [SerializeField]
        private bool waitForManagedDebugger = false;

        [SerializeField]
        private BuildOptions buildOptions = BuildOptions.StrictMode;
        public BuildOptions BuildOptions
        {
            get => buildOptions;
            protected set
            {
                if (buildOptions != value)
                {
                    buildOptions = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        [Header("Build Player")]
        [SerializeField]
        [Tooltip("Insert custom scripting defines to customise the behaviour of pre-processor macros.")]
        private ScriptingDefineSymbols scriptingDefineSymbols;

        [SerializeField]
        [Tooltip("Any custom scripting steps that should be run before making a player build.  Useful for creating extra metadata or custom build pipelines.")]
        private BuildPreparationSteps buildPreparationSteps;

        [SerializeField]
        [Tooltip("Any custom scripting steps that should be run after creating a build.  Useful for creating extra metadata or custom build pipelines.")]
        private BuildPostProcessSteps buildPostProcessSteps;

#if USE_ADDRESSABLES
        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("The addressable groups that should be built as part of this particular build setting.")]
        private AddressableGroupNames addressableGroupsInBuild;

        [Title("Build Assets")]
        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("Any custom scripting steps that should be run before building assets.  Useful for automated asset tooling.")]
        private AssetPreparationSteps buildAssetsPreparationSteps;

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("Any custom scripting steps that should be run after building assets.  Useful for automated asset tooling.")]
        private AssetPostProcessSteps buildAssetsPostProcessSteps;

        [Title("Update Assets")]
        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("Any custom scripting steps that should be run before updating assets.  Useful for automated asset tooling.")]
        private AssetPreparationSteps updateAssetsPreparationSteps;

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("Any custom scripting steps that should be run after updating assets.  Useful for automated asset tooling.")]
        private AssetPostProcessSteps updateAssetsPostProcessSteps;
#endif

        #endregion

        public void SetDefaultValues(bool isDebugConfig)
        {
            OutputName = "Build-{version}-{environment}";
            BuildDirectory = "Builds/{build_target}/{environment}";
            BuildUploadURL = "celeste-games/";
#if USE_ADDRESSABLES
            AddressablesEnabled = true;
            PlayerOverrideVersion = "resources";
            AddressablesBuildDirectory = "ServerData/{build_target}/{environment}/{major}.{minor}";
            AddressablesLoadDirectory = "https://storage.googleapis.com/celeste-games/ServerData/{build_target}/{environment}/{major}.{minor}";
            AddressablesUploadURL = "celeste-games/";
#endif
            development = isDebugConfig;
            isDebugBuild = isDebugConfig;
            buildOptions = BuildOptions.StrictMode;

            scriptingDefineSymbols = EditorOnly.FindAsset<ScriptingDefineSymbols>(isDebugBuild ? "DebugScriptingDefineSymbols" : "ReleaseScriptingDefineSymbols");
            buildPreparationSteps = EditorOnly.FindAsset<BuildPreparationSteps>();
            buildPostProcessSteps = EditorOnly.FindAsset<BuildPostProcessSteps>();
#if USE_ADDRESSABLES
            addressableGroupsInBuild = EditorOnly.FindAsset<AddressableGroupNames>();
            buildAssetsPreparationSteps = EditorOnly.FindAsset<AssetPreparationSteps>();
            buildAssetsPostProcessSteps = EditorOnly.FindAsset<AssetPostProcessSteps>("AssetPostProcessStepsForBuild");
            updateAssetsPreparationSteps = EditorOnly.FindAsset<AssetPreparationSteps>();
            updateAssetsPostProcessSteps = EditorOnly.FindAsset<AssetPostProcessSteps>("AssetPostProcessStepsForUpdate");
#endif
            SetPlatformDefaultValues(isDebugConfig);
            
            EditorUtility.SetDirty(this);
        }

        protected abstract void SetPlatformDefaultValues(bool isDebugConfig);

        #region Platform Setup Methods

        public void IncrementBuild()
        {
            if (Version != null)
            {
                Version.IncrementBuild();
                Debug.Log($"New Version is {Version} for platform {BuildTarget}");
            }
            else
            {
                Debug.LogError($"Version was null for platform settings: {name} when trying to increment.");
            }

            Apply();
        }

        public void Apply()
        {
            Debug.Log($"Applying {name}'s settings.");

#if USE_ADDRESSABLES
            if (addressablesEnabled)
            {
                SetAddressableAssetSettings();
            }
#endif

            EditorOnly.CreateFolder("Assets/Resources");
            File.WriteAllText($"Assets/Resources/{DebugConstants.IS_DEBUG_BUILD_FILE}.txt", isDebugBuild ? "1" : "0");

            ApplyImpl();

            EditorUserBuildSettings.development = development;
            EditorUserBuildSettings.waitForManagedDebugger = waitForManagedDebugger;
            PlayerSettings.bundleVersion = Version.ToString();
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup, scriptingDefineSymbols.ToArray());

#if USE_ADDRESSABLES
            if (addressablesEnabled)
            {
                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
                settings.OverridePlayerVersion = PlayerOverrideVersion;
                AddressablesExtensions.SetAddressablesRemoteBuildPath(AddressablesBuildDirectory);
                AddressablesExtensions.SetAddressablesRemoteLoadPath(AddressablesLoadDirectory);
                AddressablesExtensions.SetAddressablesLocalBuildPath(LocalAddressablesBuildPath);
                AddressablesExtensions.SetAddressablesLocalLoadPath(LocalAddressablesLoadPath);

                if (addressableGroupsInBuild.NumItems > 0)
                {
                    settings.DefaultGroup = settings.FindGroup(addressableGroupsInBuild.GetItem(0));
                }

                foreach (AddressableAssetGroup group in settings.groups)
                {
                    BundledAssetGroupSchema bundledAssetGroupSchema = group.GetSchema<BundledAssetGroupSchema>();
                    bool included = addressableGroupsInBuild.Contains(group.Name);

                    if (bundledAssetGroupSchema != null && bundledAssetGroupSchema.IncludeInBuild != included)
                    {
                        bundledAssetGroupSchema.IncludeInBuild = included;
                        EditorUtility.SetDirty(group);
                    }
                }
            }
#endif

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        protected abstract void ApplyImpl();

        public void Switch()
        {
            Debug.Log($"Switching Active Editor Build Target to {BuildTargetGroup} - {BuildTarget}.");
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup, BuildTarget);
            Apply();
        }

#endregion

        #region Building

        public BuildPlayerOptions GenerateBuildPlayerOptions()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.options = BuildOptions;
            buildPlayerOptions.locationPathName = EditorOnly.EnsureDelimitersCorrect(Path.Combine(BuildDirectory, OutputName));
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(x => x.path).ToArray();
            buildPlayerOptions.target = BuildTarget;
            buildPlayerOptions.targetGroup = BuildTargetGroup;

            return ModifyBuildPlayerOptions(buildPlayerOptions);
        }

        protected virtual BuildPlayerOptions ModifyBuildPlayerOptions(BuildPlayerOptions buildPlayerOptions)
        {
            return buildPlayerOptions;
        }

        public void BuildPlayer()
        {
            BuildPlayer(GenerateBuildPlayerOptions());
        }

        public void PrepareForBuild(BuildPlayerOptions buildPlayerOptions)
        {
            foreach (BuildPreparationStep buildPreparationStep in buildPreparationSteps)
            {
                buildPreparationStep.Execute(buildPlayerOptions, this);
            }
        }

        public void BuildPlayer(BuildPlayerOptions buildPlayerOptions)
        {
            LogExtensions.Clear();

            Switch();
#if USE_ADDRESSABLES
            BuildAssets();  // Always build assets, as the latest addressables data must be in the build
#endif
            Debug.Log($"Location Path Name: {buildPlayerOptions.locationPathName}");

            PrepareForBuild(buildPlayerOptions);

            BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            bool success = buildReport != null && buildReport.summary.result == BuildResult.Succeeded;

            foreach (BuildPostProcessStep buildPostProcessStep in buildPostProcessSteps)
            {
                if (success || !buildPostProcessStep.OnlyExecuteOnSuccess)
                {
                    buildPostProcessStep.Execute(buildPlayerOptions, buildReport, this);
                }
            }

            if (success)
            {
                PersistenceMenuItemUtility.OpenWithDefaultApp(BuildDirectory);
            }
            else if (Application.isBatchMode)
            {
                EditorApplication.Exit(1);
            }
        }

        public void InjectBuildEnvVars(StringBuilder stringBuilder)
        {
            DoInjectBuildEnvVars(stringBuilder);
        }

        protected virtual void DoInjectBuildEnvVars(StringBuilder stringBuilder) { }

#if USE_ADDRESSABLES
        public void PrepareAssetsForBuild()
        {
            foreach (AssetPreparationStep assetPreparationStep in buildAssetsPreparationSteps)
            {
                assetPreparationStep.Execute();
            }
        }

        public void PrepareAssetsForUpdate()
        {
            foreach (AssetPreparationStep assetPreparationStep in updateAssetsPreparationSteps)
            {
                assetPreparationStep.Execute();
            }
        }

        private void PostProcessAssetsForBuild(AddressablesPlayerBuildResult result)
        {
            foreach (AssetPostProcessStep assetPostProcessStep in buildAssetsPostProcessSteps)
            {
                if (string.IsNullOrEmpty(result.Error) || !assetPostProcessStep.OnlyExecuteOnSuccess)
                {
                    assetPostProcessStep.Execute(result, this);
                }
            }
        }

        private void PostProcessAssetsForUpdate(AddressablesPlayerBuildResult result)
        {
            foreach (AssetPostProcessStep assetPostProcessStep in updateAssetsPostProcessSteps)
            {
                if (string.IsNullOrEmpty(result.Error) || !assetPostProcessStep.OnlyExecuteOnSuccess)
                {
                    assetPostProcessStep.Execute(result, this);
                }
            }
        }

        public void BuildAssets()
        {
            LogExtensions.Clear();
            
            Switch();
            PrepareAssetsForBuild();

            Debug.Log("Beginning to build content");
            AddressableAssetSettings.BuildPlayerContent(out var result);

            PostProcessAssetsForBuild(result);
            Debug.Log("Finished building content");
        }

        public bool UpdateAssets()
        {
            LogExtensions.Clear();
            
            Switch();
            PrepareAssetsForUpdate();

            Debug.Log("Beginning to update content");

            string contentStatePath = ContentUpdateScript.GetContentStateDataPath(false);
            Debug.Log($"Using content state path {contentStatePath}");
            AddressablesPlayerBuildResult buildResult = ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, contentStatePath);

            if (buildResult != null)
            {
                Debug.Log($"Finished updating content{(string.IsNullOrEmpty(buildResult.Error) ? "" : $" with error: {buildResult.Error}")}");
            }
            else
            {
                Debug.Log("Finished updating content with no build result");
            }
            
            PostProcessAssetsForUpdate(buildResult);

            return buildResult != null && string.IsNullOrEmpty(buildResult.Error);
        }

        private static void SetAddressableAssetSettings()
        {
            if (AddressableAssetSettingsDefaultObject.Settings == null)
            {
                Debug.Log("Loading settings from asset database");
                AddressableAssetSettingsDefaultObject.Settings = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(AddressableAssetSettingsDefaultObject.DefaultAssetPath);
            }

            Debug.Assert(AddressableAssetSettingsDefaultObject.Settings != null, "AddressableAssetSettingsDefaultObject is null");
        }
#endif

#endregion

        #region Version Methods

        protected static Version ParseVersion(int bundleVersionCode)
        {
            int major = bundleVersionCode / 10000;
            int minor = (bundleVersionCode - major * 10000) / 100;
            int patch = bundleVersionCode - major * 10000 - minor * 100;

            return new Version(major, minor, patch);
        }

        protected static Version ParseVersion(string bundleString)
        {
            return new Version(bundleString);
        }

        private string Resolve(string stringWithPossibleVersionCodes)
        {
            return string.IsNullOrEmpty(stringWithPossibleVersionCodes) ? 
                stringWithPossibleVersionCodes :
                stringWithPossibleVersionCodes.
                    Replace("{version}", Version.ToString()).
                    Replace("{major}", Version.Major.ToString()).
                    Replace("{minor}", Version.Minor.ToString()).
                    Replace("{build}", Version.Build.ToString()).
                    Replace("{build_target}", BuildTarget.ToString()).
                    Replace("{build_target_group}", BuildTargetGroup.ToString()).
                    Replace("{environment}", isDebugBuild ? "Debug" : "Release");
        }

        #endregion
    }
}
