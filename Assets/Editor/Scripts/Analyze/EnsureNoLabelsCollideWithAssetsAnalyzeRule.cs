#if USE_ADDRESSABLES
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;

namespace CelesteEditor.Assets.Analyze
{
    [InitializeOnLoad]
    public static class RegisterEnsureNoLabelsCollideWithAssetsAnalyzeRule
    {
        static RegisterEnsureNoLabelsCollideWithAssetsAnalyzeRule()
        {
            AnalyzeSystem.RegisterNewRule<EnsureNoLabelsCollideWithAssetsAnalyzeRule>();
        }
    }

    public class EnsureNoLabelsCollideWithAssetsAnalyzeRule : AnalyzeRule
    {
        #region Properties and Fields

        public override string ruleName => "Ensure No Labels Collide With Assets";
        public override bool CanFix { get; set; } = false;

        #endregion

        #region Analyze

        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings)
        {
            List<AnalyzeResult> analyzeResults = new List<AnalyzeResult>();

            foreach (AddressableAssetEntry assetEntry in Analyze(settings))
            {
                AddAnalyzeResult(assetEntry, analyzeResults);
            }

            return analyzeResults;
        }

        private List<AddressableAssetEntry> Analyze(AddressableAssetSettings settings)
        {
            HashSet<string> labels = new HashSet<string>(settings.GetLabels());
            List<AddressableAssetEntry> brokenAssetEntries = new List<AddressableAssetEntry>();

            foreach (AddressableAssetGroup addressableGroup in settings.groups)
            {
                foreach (var assetEntry in addressableGroup.entries)
                {
                    if (labels.Contains(assetEntry.address))
                    {
                        brokenAssetEntries.Add(assetEntry);
                    }
                }
            }

            return brokenAssetEntries;
        }

        private void AddAnalyzeResult(AddressableAssetEntry assetEntry, List<AnalyzeResult> analyzeResults)
        {
            analyzeResults.Add(new AnalyzeResult()
            {
                resultName = $"'{assetEntry.TargetAsset.name}' has an address which is also a label ({assetEntry.address})",
                severity = MessageType.Error
            });
        }

        #endregion
    }
}
#endif