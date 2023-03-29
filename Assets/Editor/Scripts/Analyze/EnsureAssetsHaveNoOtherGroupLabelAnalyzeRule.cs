using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;

namespace CelesteEditor.Assets.Analyze
{
    [InitializeOnLoad]
    public static class RegisterEnsureAssetsHaveNoOtherGroupLabelAnalyzeRule
    {
        static RegisterEnsureAssetsHaveNoOtherGroupLabelAnalyzeRule()
        {
            AnalyzeSystem.RegisterNewRule<EnsureAssetsHaveNoOtherGroupLabelAnalyzeRule>();
        }
    }

    public class EnsureAssetsHaveNoOtherGroupLabelAnalyzeRule : AnalyzeRule
    {
        #region Properties and Fields

        public override string ruleName => "Ensure Assets Have No Other Group Label";
        public override bool CanFix { get; set; } = true;

        #endregion

        #region Analyze

        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings)
        {
            HashSet<string> addressableGroupNames = new HashSet<string>();
            foreach (var group in settings.groups)
            {
                addressableGroupNames.Add(group.Name);
            }

            List<AnalyzeResult> analyzeResults = new List<AnalyzeResult>();

            foreach (AddressableAssetEntry assetEntry in Analyze(settings, addressableGroupNames))
            {
                AddAnalyzeResult(assetEntry, analyzeResults);
            }

            CanFix = true;

            return analyzeResults;
        }

        private List<AddressableAssetEntry> Analyze(AddressableAssetSettings settings, HashSet<string> addressableGroupNames)
        {
            List<AddressableAssetEntry> assetEntriesToFix = new List<AddressableAssetEntry>();

            foreach (AddressableAssetGroup addressableGroup in settings.groups)
            {
                HashSet<string> allOtherGroupNames = new HashSet<string>(addressableGroupNames);
                allOtherGroupNames.Remove(addressableGroup.Name);

                foreach (var assetEntry in addressableGroup.entries)
                {
                    if (assetEntry.TargetAsset != null && assetEntry.labels.Overlaps(allOtherGroupNames))
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
                resultName = $"'{assetEntry.TargetAsset.name}' has a label from another group ({assetEntry.address})",
                severity = MessageType.Error
            });
        }

        #endregion

        #region Fix

        public override void FixIssues(AddressableAssetSettings settings)
        {
            HashSet<string> addressableGroupNames = new HashSet<string>();
            foreach (var group in settings.groups)
            {
                addressableGroupNames.Add(group.Name);
            }

            foreach (var assetEntryToFix in Analyze(settings, addressableGroupNames))
            {
                // Go through each label and remove any other addressable group names
                foreach (string label in addressableGroupNames)
                {
                    if (string.CompareOrdinal(label, assetEntryToFix.parentGroup.Name) != 0)
                    {
                        assetEntryToFix.SetLabel(label, false);
                    }
                }
            }

            ClearAnalysis();
        }

        #endregion
    }
}