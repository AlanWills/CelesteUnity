using System.Collections.Generic;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;

namespace CelesteEditor.Assets.Analyze
{
    public abstract class AddressableGroupAnalyzeRule : AnalyzeRule
    {
        #region Analyze

        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings)
        {
            List<AnalyzeResult> analyzeResults = new List<AnalyzeResult>();

            foreach (AddressableAssetGroup assetGroup in Analyze(settings))
            {
                AddAnalyzeResult(assetGroup, analyzeResults);
            }

            return analyzeResults;
        }

        private List<AddressableAssetGroup> Analyze(AddressableAssetSettings settings)
        {
            List<AddressableAssetGroup> addressableGroupsToFix = new List<AddressableAssetGroup>();

            foreach (AddressableAssetGroup addressableGroup in settings.groups)
            {
                if (ViolatesRule(addressableGroup))
                {
                    addressableGroupsToFix.Add(addressableGroup);
                }
            }

            return addressableGroupsToFix;
        }

        protected abstract bool ViolatesRule(AddressableAssetGroup addressableAssetGroup);
        protected abstract void AddAnalyzeResult(AddressableAssetGroup assetGroup, List<AnalyzeResult> analyzeResults);

        #endregion

        #region Fix

        public override void FixIssues(AddressableAssetSettings settings)
        {
            foreach (var assetGroupToFix in Analyze(settings))
            {
                FixIssue(assetGroupToFix);
            }

            ClearAnalysis();
        }

        protected abstract void FixIssue(AddressableAssetGroup addressableAssetGroup);

        #endregion
    }
}