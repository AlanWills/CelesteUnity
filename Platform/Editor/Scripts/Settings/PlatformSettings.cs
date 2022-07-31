using Celeste.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using CelesteEditor.Tools;
using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Scene;
using CelesteEditor.BuildSystem.Steps;
using CelesteEditor.BuildSystem.Data;

namespace CelesteEditor.BuildSystem
{
    public abstract class PlatformSettings : ScriptableObject
    {
        #region Properties and Fields

        private const string STRING_SUBSTITUTION_HELP = "  {version} will be replaced with the full version number.  {major}, {minor} and {build} will be replaced with the corresponding values from the version.";

        [SerializeField]
        [Tooltip("The version number that corresponds to the Application.version string.  Usually of the form 'Major.Minor.Patch'.")]
        private AppVersion version;
        public AppVersion Version
        {
            get { return version; }
            protected set
            {
                version = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        [Tooltip("The directory relative to the project directory that the build will be created in." + STRING_SUBSTITUTION_HELP)]
        private string buildDirectory;
        public string BuildDirectory
        {
            get { return Resolve(buildDirectory); }
            protected set
            {
                buildDirectory = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        [Tooltip("When integrating build upload to google drive through a pipeline like Jenkins, the variable 'BUILD_GDRIVE_UPLOAD_DIRECTORY' will be added to an environment variables file set to this value.")]
        private string gDriveBuildUploadDirectory;

        [SerializeField]
        [Tooltip("The name of the outputted build." + STRING_SUBSTITUTION_HELP)]
        private string outputName;
        public string OutputName
        {
            get { return Resolve(outputName); }
            protected set
            {
                outputName = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        [Tooltip("If enabled, addressable specific pipelines will run in the build pipeline.")]
        private bool addressablesEnabled = false;

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("Used to target the addressables at a different version to the 'version' variable e.g. if you wanted addressables for '0.3.x'." + STRING_SUBSTITUTION_HELP)]
        private string playerOverrideVersion;
        public string PlayerOverrideVersion
        {
            get { return Resolve(playerOverrideVersion); }
            protected set
            {
                playerOverrideVersion = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("The directory that build addressables will be outputted to, relative to the project directory.  " +
            "When building addressables as part of a build pipeline, this value will be added to a file under the variable 'ASSETS_SOURCE' to allow uploading from a specific location." + STRING_SUBSTITUTION_HELP)]
        private string addressablesBuildDirectory;
        public string AddressablesBuildDirectory
        {
            get { return Resolve(addressablesBuildDirectory); }
        }

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("The remote load path for the addressables e.g. an S3 bucket." + STRING_SUBSTITUTION_HELP)]
        private string addressablesLoadDirectory;
        public string AddressablesLoadDirectory
        {
            get { return Resolve(addressablesLoadDirectory); }
        }

        [SerializeField, ShowIf(nameof(addressablesEnabled))]
        [Tooltip("When building addressables as part of a build pipeline, this value will be added to a file under the variable 'ASSETS_DESTINATION' to allow uploading to a specific location." + STRING_SUBSTITUTION_HELP)]
        private string addressablesS3UploadBucket;
        public string AddressablesS3UploadBucket
        {
            get { return Resolve(addressablesS3UploadBucket); }
        }

        [SerializeField]
        private BuildTarget buildTarget;
        public BuildTarget BuildTarget
        {
            get { return buildTarget; }
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
            get { return buildTargetGroup; }
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
        private BuildOptions buildOptions = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.StrictMode;
        public BuildOptions BuildOptions
        {
            get { return buildOptions; }
        }

        [SerializeField]
        [Tooltip("Insert custom scripting defines to customise the behaviour of pre-processor macros.")]
        private ScriptingDefineSymbols scriptingDefineSymbols;

        [SerializeField]
        [Tooltip("The addressable groups that should be built as part of this particular build setting.")]
        private AddressableGroupNames addressableGroupsInBuild;

        [SerializeField]
        [Tooltip("Any custom scripting steps that should be run before building assets.  Useful for automated asset tooling.")]
        private AssetPreparationSteps assetPreparationSteps;

        #endregion

        public abstract void SetDefaultValues();

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
            if (addressablesEnabled)
            {
                SetAddressableAssetSettings();
            }

            AssetUtility.CreateFolder("Assets/Resources");
            File.WriteAllText($"Assets/Resources/{DebugConstants.IS_DEBUG_BUILD_FILE}.txt", isDebugBuild ? "1" : "0");

            ApplyImpl();

            EditorUserBuildSettings.development = development;
            PlayerSettings.bundleVersion = Version.ToString();
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup, scriptingDefineSymbols.ToArray());

            if (addressablesEnabled)
            {
                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
                settings.OverridePlayerVersion = PlayerOverrideVersion;
                settings.profileSettings.SetValue(settings.activeProfileId, "RemoteBuildPath", AddressablesBuildDirectory);
                settings.profileSettings.SetValue(settings.activeProfileId, "RemoteLoadPath", AddressablesLoadDirectory);

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

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        protected abstract void ApplyImpl();

        public void Switch()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup, BuildTarget);
            Apply();
        }

        #endregion

        #region Building

        public void BuildPlayer()
        {
            Switch();
            BuildAssets();  // Always build assets, as the latest addressables data must be in the build

            string buildDirectory = BuildDirectory;
            string outputName = OutputName;

            Debug.Log($"Build Directory: {buildDirectory}");
            Debug.Log($"Output Name: {outputName}");

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.options = BuildOptions;
            buildPlayerOptions.locationPathName = Path.Combine(buildDirectory, outputName);
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(x => x.path).ToArray();
            buildPlayerOptions.target = BuildTarget;
            buildPlayerOptions.targetGroup = BuildTargetGroup;

            BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            bool success = buildReport != null && buildReport.summary.result == BuildResult.Succeeded;

            if (success)
            {
                StringBuilder buildInfo = new StringBuilder();
                buildInfo.Append($"BUILD_LOCATION={buildPlayerOptions.locationPathName}");
                buildInfo.AppendLine();
                buildInfo.Append($"BUILD_VERSION={version}"); 
                buildInfo.AppendLine();
                buildInfo.Append($"BUILD_GDRIVE_UPLOAD_DIRECTORY={gDriveBuildUploadDirectory}");
                InjectBuildEnvVars(buildInfo);
                File.WriteAllText(Path.Combine(buildDirectory, "BUILD_ENV_VARS.txt"), buildInfo.ToString());

                MenuItemUtility.OpenExplorerAt(buildDirectory);
            }
            else if (Application.isBatchMode)
            {
                EditorApplication.Exit(1);
            }
        }

        protected virtual void InjectBuildEnvVars(StringBuilder stringBuilder) { }

        public void PrepareAssets()
        {
            foreach (AssetPreparationStep assetPreparationStep in assetPreparationSteps)
            {
                assetPreparationStep.Execute();
            }
        }

        public void BuildAssets()
        {
            PrepareAssets();
            Switch();

            Debug.Log("Beginning to build content");
            AddressableAssetSettings.BuildPlayerContent();

            StringBuilder locationInfo = new StringBuilder();
            locationInfo.Append($"ASSETS_SOURCE={AddressablesBuildDirectory}/*");
            locationInfo.AppendLine();
            locationInfo.Append($"ASSETS_DESTINATION={AddressablesS3UploadBucket}");

            if (!Directory.Exists(AddressablesBuildDirectory))
            {
                Directory.CreateDirectory(AddressablesBuildDirectory);
            }
            File.WriteAllText(Path.Combine(new DirectoryInfo(AddressablesBuildDirectory).Parent.FullName, "ASSETS_ENV_VARS.txt"), locationInfo.ToString());

            Debug.Log("Finished building content");
        }

        public bool UpdateAssets()
        {
            PrepareAssets();
            Switch();

            Debug.Log("Beginning to update content");

            string contentStatePath = ContentUpdateScript.GetContentStateDataPath(false);
            Debug.Log($"Using content state path {contentStatePath}");
            AddressableAssetBuildResult buildResult = ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, contentStatePath);

            if (buildResult != null)
            {
                Debug.Log($"Finished updating content{(string.IsNullOrEmpty(buildResult.Error) ? "" : $" with error: {buildResult.Error}")}");
            }
            else
            {
                Debug.Log("Finished updating content with no build result");
            }

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
            return stringWithPossibleVersionCodes.
                    Replace("{version}", Version.ToString()).
                    Replace("{major}", Version.Major.ToString()).
                    Replace("{minor}", Version.Minor.ToString()).
                    Replace("{build}", Version.Build.ToString());
        }

        #endregion
    }
}
