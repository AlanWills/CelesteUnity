using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CelesteEditor.BuildSystem
{
    [CustomEditor(typeof(AndroidSettings))]
    [CanEditMultipleObjects]
    public class AndroidSettingsEditor : PlatformSettingsEditor
    {
        [Flags]
        private enum AndroidArchitectureFlags
        {
            //
            // Summary:
            //     32-bit ARM architecture.
            ARMv7 = 1 << 0,
            //
            // Summary:
            //     64-bit ARM architecture.
            ARM64 = 1 << 1,
            //
            // Summary:
            //     32-bit Intel architecture.
            X86 = 1 << 2,
            #if !UNITY_6000_0_OR_NEWER
            //
            // Summary:
            //     64-bit Intel architecture.
            X86_64 = 1 << 3
            #endif
        }

        protected override void DoOnEnable()
        {
            base.DoOnEnable();


            AddDrawPropertyCallback("architecture", property =>
            {
                AndroidArchitecture currentArchitecture = (AndroidArchitecture)property.uintValue;
                currentArchitecture = AndroidArchitectureSelect(currentArchitecture, AndroidArchitecture.ARMv7);
                currentArchitecture = AndroidArchitectureSelect(currentArchitecture, AndroidArchitecture.ARM64);
                #if !UNITY_6000_0_OR_NEWER
                currentArchitecture = AndroidArchitectureSelect(currentArchitecture, AndroidArchitecture.X86);
                #endif
                currentArchitecture = AndroidArchitectureSelect(currentArchitecture, AndroidArchitecture.X86_64);

                property.uintValue = (uint)currentArchitecture;
            });

            AddDrawPropertyCallback("minSdkVersion", property =>
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    property.intValue = EditorGUILayout.IntField("Min SDK Version", property.intValue);
                }
            });

            AddDrawPropertyCallback("targetSdkVersion", property =>
            {
                bool useAutoSDKVersion = property.intValue == (int)AndroidSdkVersions.AndroidApiLevelAuto;
                bool newUseAutoSDKVersion = EditorGUILayout.Toggle("Auto Detect Target SDK Version", useAutoSDKVersion);

                if (newUseAutoSDKVersion)
                {
                    property.intValue = (int)AndroidSdkVersions.AndroidApiLevelAuto;
                }
                else
                {
                    if (useAutoSDKVersion != newUseAutoSDKVersion)
                    {
                        // We've just switched from auto to not, so we need to set this to a reasonable default value
                        property.intValue = AndroidSettings.DEFAULT_TARGET_SDK_VERSION;
                    }

                    property.intValue = EditorGUILayout.IntField("Target SDK Version", property.intValue);
                }
            });
        }

        private static AndroidArchitecture AndroidArchitectureSelect(AndroidArchitecture currentArchitecture, AndroidArchitecture targetArchitecture)
        {
            bool buildForTarget = (currentArchitecture & targetArchitecture) == targetArchitecture;
            bool newBuildForTarget = EditorGUILayout.Toggle($"Build For {targetArchitecture}", buildForTarget);

            if (buildForTarget != newBuildForTarget)
            {
                currentArchitecture ^= targetArchitecture;
            }

            return currentArchitecture;
        }
    }
}
