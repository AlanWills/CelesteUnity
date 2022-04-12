using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;

namespace CelesteEditor.Assets.Analyze
{
    [InitializeOnLoad]
    public static class RegisterEnsureAssetsConfiguredCorrectlyAnalyzeRule
    {
        static RegisterEnsureAssetsConfiguredCorrectlyAnalyzeRule()
        {
            AnalyzeSystem.RegisterNewRule<EnsureAssetsConfiguredCorrectlyAnalyzeRule>();
        }
    }

    public class EnsureAssetsConfiguredCorrectlyAnalyzeRule : AnalyzeRule
    {
        #region Properties and Fields

        public override string ruleName => "Ensure Assets Configured Correctly";
        public override bool CanFix { get; set; } = true;

        #endregion

        #region Analyze

        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings)
        {
            List<AnalyzeResult> analyzeResults = new List<AnalyzeResult>();

            foreach (AddressableAssetEntry assetEntry in Analyze(settings))
            {
                AddAnalyzeResult(assetEntry, analyzeResults);
            }

            CanFix = true;

            return analyzeResults;
        }

        private List<AddressableAssetEntry> Analyze(AddressableAssetSettings settings)
        {
            List<AddressableAssetEntry> assetEntriesToFix = new List<AddressableAssetEntry>();

            foreach (AddressableAssetGroup addressableGroup in settings.groups)
            {
                foreach (var assetEntry in addressableGroup.entries)
                {
                    if (assetEntry.TargetAsset != null && !assetEntry.labels.Contains(addressableGroup.Name))
                    {
                        assetEntriesToFix.Add(assetEntry);
                    }
                }
            }

            return assetEntriesToFix;
        }

        private void AddAnalyzeResult(AddressableAssetEntry assetEntry, List<AnalyzeResult> analyzeResults)
        {
            analyzeResults.Add(new AnalyzeResult()
            {
                resultName = $"'{assetEntry.TargetAsset.name}' does not have the corresponding group tag '{assetEntry.parentGroup.Name}' ({assetEntry.address})",
                severity = MessageType.Error
            });
        }

        #endregion

        #region Fix

        public override void FixIssues(AddressableAssetSettings settings)
        {
            foreach (var assetEntryToFix in Analyze(settings))
            {
                assetEntryToFix.SetLabel(assetEntryToFix.parentGroup.Name, true);
            }

            ClearAnalysis();
        }

        #endregion
    }
}