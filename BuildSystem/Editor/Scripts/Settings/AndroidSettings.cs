﻿using Celeste;
using System;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CreateAssetMenu(
        fileName = "AndroidSettings", 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Android Settings",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class AndroidSettings : PlatformSettings
    {
        #region Properties and Fields

        // HideInInspector cos enum not editable via default UI
        [SerializeField, HideInInspector]
        private ScriptingImplementation scriptingBackend;
        public ScriptingImplementation ScriptingBackend
        {
            get { return scriptingBackend; }
            set
            {
                scriptingBackend = value;
                EditorUtility.SetDirty(this);
            }
        }

        // HideInInspector cos enum not editable via default UI
        [SerializeField, HideInInspector]
        private AndroidArchitecture architecture;
        public AndroidArchitecture Architecture
        {
            get { return architecture; }
            set 
            { 
                architecture = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField, Header("Android Settings")]
        [Tooltip("Whether the build pipeline should create an apk or aab.  Set this to true if building for store release, as only an aab can be uploading to Google Play.")]
        private bool buildAppBundle;
        private bool BuildAppBundle
        {
            get { return buildAppBundle; }
        }

        [SerializeField]
        [Tooltip("A flag to instruct the build pipeline to create debug symbols to help symbolicate crashes?")]
        private AndroidCreateSymbols buildSymbols = AndroidCreateSymbols.Disabled;
        private AndroidCreateSymbols BuildSymbols
        {
            get { return buildSymbols; }
        }

        [SerializeField]
        [Tooltip("A flag to indicate if the build pipeline strip out unused code to reduce app size.")]
        private bool minifyRelease;
        public bool MinifyRelease
        {
            get { return minifyRelease; }
        }

        [SerializeField]
        [Tooltip("The password for the local keystore used to sign the build.")]
        private string keystorePassword;
        public string KeystorePassword
        {
            get { return keystorePassword; }
        }

        [SerializeField]
        [Tooltip("The key used to sign the build.")]
        private string keyAliasName;
        public string KeyAliasName
        {
            get { return keyAliasName; }
        }

        [SerializeField]
        [Tooltip("The password for the key used to sign the build.")]
        private string keyAliasPassword;
        public string KeyAliasPassword
        {
            get { return keyAliasPassword; }
        }

        [SerializeField]
        [Tooltip("If true, the permission to access the SD card for writing will be prompted to the user.")]
        private bool requiresWritePermission;
        public bool RequiresWritePermission
        {
            get => requiresWritePermission;
            set
            {
                requiresWritePermission = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        [Tooltip("The minimum Android Sdk version that this build will be able to target.")]
        private AndroidSdkVersions minSdkVersion;
        public AndroidSdkVersions MinSdkVersion
        {
            get => minSdkVersion;
            set
            {
                minSdkVersion = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        [Tooltip("The desired Android Sdk version that this build will be able to target.")]
        private AndroidSdkVersions targetSdkVersion;
        public AndroidSdkVersions TargetSdkVersion
        {
            get => targetSdkVersion;
            set
            {
                targetSdkVersion = value;
                EditorUtility.SetDirty(this);
            }
        }

        #endregion

        public override void SetDefaultValues()
        {
            BuildDirectory = "Builds/Android";
            OutputName = "Build-{version}.apk";
            BuildTarget = BuildTarget.Android;
            BuildTargetGroup = BuildTargetGroup.Android;
            ScriptingBackend = ScriptingImplementation.IL2CPP;
            Architecture = AndroidArchitecture.ARMv7;
            RequiresWritePermission = true;
            MinSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
            TargetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        }

        protected override void ApplyImpl()
        {
            PlayerSettings.Android.bundleVersionCode = Version.Major * 10000 + Version.Minor * 100 + Version.Build;
            Debug.Log($"Android version is now: {Version}");

            PlayerSettings.Android.keystorePass = KeystorePassword;
            PlayerSettings.Android.keyaliasName = KeyAliasName;
            PlayerSettings.Android.keyaliasPass = KeyAliasPassword;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingBackend);
            PlayerSettings.Android.targetArchitectures = Architecture;
            PlayerSettings.Android.minifyRelease = MinifyRelease;
            EditorUserBuildSettings.buildAppBundle = BuildAppBundle;
            EditorUserBuildSettings.androidCreateSymbols = BuildSymbols;
            PlayerSettings.Android.forceSDCardPermission = RequiresWritePermission;
            PlayerSettings.Android.minSdkVersion = MinSdkVersion;
            PlayerSettings.Android.targetSdkVersion = TargetSdkVersion;
        }
    }
}
