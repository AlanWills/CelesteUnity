using System.Collections.Generic;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;

namespace CelesteEditor.Assets.Analyze
{
    public abstract class AddressableAssetAnalyzeRule : AnalyzeRule
    {
        #region Analyze

        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings)
        {
            List<AnalyzeResult> analyzeResults = new List<AnalyzeResult>();

            foreach (AddressableAssetGroup assetGroup in Analyze(settings))
            {
                foreach (var addressableAssetEntry in assetGroup.entries)
                {
                    FixIssue(addressableAssetEntry);
                }
            }

            return analyzeResults;
        }

        private List<AddressableAssetGroup> Analyze(AddressableAssetSettings settings)
        {
            List<AddressableAssetGroup> addressableGroupsToFix = new List<AddressableAssetGroup>();

            foreach (AddressableAssetGroup addressableGroup in settings.groups)
            {
                foreach (var addressableAssetEntry in addressableGroup.entries)
                {
                    if (ViolatesRule(addressableAssetEntry))
                    {
                        addressableGroupsToFix.Add(addressableGroup);
                    }
                }
            }

            return addressableGroupsToFix;
        }

        protected abstract bool ViolatesRule(AddressableAssetEntry addressableAssetEntry);
        protected abstract void AddAnalyzeResult(AddressableAssetEntry addressableAssetEntry, List<AnalyzeResult> analyzeResults);

        #endregion

        #region Fix

        public override void FixIssues(AddressableAssetSettings settings)
        {
            foreach (var assetGroupToFix in Analyze(settings))
            {
                foreach (var addressableAssetEntry in assetGroupToFix.entries)
                {
                    FixIssue(addressableAssetEntry);
                }
            }

            ClearAnalysis();
        }

        protected abstract void FixIssue(AddressableAssetEntry addressableAssetEntry);

        #endregion
    }
}