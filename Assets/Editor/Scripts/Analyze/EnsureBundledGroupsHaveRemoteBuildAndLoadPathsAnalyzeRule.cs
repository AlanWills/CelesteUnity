using CelesteEditor.Assets.Schemas;
using CelesteEditor.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace CelesteEditor.Assets.Analyze
{
    [InitializeOnLoad]
    public static class RegisterEnsureBundledGroupsHaveRemoteBuildAndLoadPathsAnalyzeRule
    {
        static RegisterEnsureBundledGroupsHaveRemoteBuildAndLoadPathsAnalyzeRule()
        {
            AnalyzeSystem.RegisterNewRule<EnsureBundledGroupsHaveRemoteBuildAndLoadPathsAnalyzeRule>();
        }
    }

    public class EnsureBundledGroupsHaveRemoteBuildAndLoadPathsAnalyzeRule : AddressableGroupAnalyzeRule
    {
        public override string ruleName => "Ensure Bundled Groups Have Remote Build and Load Paths";
        public override bool CanFix { get; set; } = true;

        protected override bool ViolatesRule(AddressableAssetGroup addressableAssetGroup)
        {
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            string remoteBuildPath = AddressablesUtility.GetAddressablesRemoteBuildPath();
            string remoteLoadPath = AddressablesUtility.GetAddressablesRemoteLoadPath();

            if (addressableAssetGroup.TryGetSchema<BundledGroupSchema>(out var bundledGroup) &&
                addressableAssetGroup.TryGetSchema<BundledAssetGroupSchema>(out var bundledAssets) &&
                bundledGroup.BundleInStreamingAssets)
            {
                bool remoteBuildPathIsDifferent = string.CompareOrdinal(remoteBuildPath, bundledAssets.BuildPath.GetValue(addressableSettings)) != 0;
                bool remoteLoadPathIsDifferent = string.CompareOrdinal(remoteLoadPath, bundledAssets.LoadPath.GetValue(addressableSettings)) != 0;
                return remoteBuildPathIsDifferent || remoteLoadPathIsDifferent;
            }

            return false;
        }

        protected override void AddAnalyzeResult(AddressableAssetGroup assetGroup, List<AnalyzeResult> analyzeResults)
        {
            analyzeResults.Add(new AnalyzeResult()
            {
                resultName = $"'{assetGroup.name}' does not have the remote build or load paths.  These must be set correctly or the addressables build pipeline will not function correctly.",
                severity = MessageType.Error
            });
        }

        protected override void FixIssue(AddressableAssetGroup addressableAssetGroup)
        {
            string remoteBuildPath = AddressablesUtility.GetAddressablesRemoteBuildPath();
            string remoteLoadPath = AddressablesUtility.GetAddressablesRemoteLoadPath();

            addressableAssetGroup.SetBuildPath(remoteBuildPath);
            addressableAssetGroup.SetLoadPath(remoteLoadPath);
        }
    }
}