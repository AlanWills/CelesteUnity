#if USE_ADDRESSABLES
using CelesteEditor.Assets.Schemas;
using System.Collections.Generic;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace CelesteEditor.Assets.Analyze
{
    [InitializeOnLoad]
    public static class RegisterEnsureGroupsHaveBundledSchemaAnalyzeRule
    {
        static RegisterEnsureGroupsHaveBundledSchemaAnalyzeRule()
        {
            AnalyzeSystem.RegisterNewRule<EnsureGroupsHaveBundledSchemaAnalyzeRule>();
        }
    }

    public class EnsureGroupsHaveBundledSchemaAnalyzeRule : AnalyzeRule
    {
        #region Properties and Fields

        public override string ruleName => "Ensure Groups Have Bundled Schema";
        public override bool CanFix { get; set; } = true;

        #endregion

        #region Analyze

        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings)
        {
            List<AnalyzeResult> analyzeResults = new List<AnalyzeResult>();

            foreach (AddressableAssetGroup assetGroup in Analyze(settings))
            {
                AddAnalyzeResult(assetGroup, analyzeResults);
            }

            CanFix = true;

            return analyzeResults;
        }

        private List<AddressableAssetGroup> Analyze(AddressableAssetSettings settings)
        {
            List<AddressableAssetGroup> addressableGroupsToFix = new List<AddressableAssetGroup>();

            foreach (AddressableAssetGroup addressableGroup in settings.groups)
            {
                // Don't care about the Resources and Built in Data that comes default with Addressables
                if (addressableGroup.HasSchema<PlayerDataGroupSchema>())
                {
                    continue;
                }

                if (addressableGroup.HasSchema<BundledGroupSchema>())
                {
                    continue;
                }
                
                BundledAssetGroupSchema bundledAssetGroupSchema = addressableGroup.GetSchema<BundledAssetGroupSchema>();
                if (bundledAssetGroupSchema == null)
                {
                    continue;
                }

                if (bundledAssetGroupSchema.HasRemoteBuildPath(addressableGroup.Settings))
                {
                    addressableGroupsToFix.Add(addressableGroup);
                }
            }

            return addressableGroupsToFix;
        }

        private void AddAnalyzeResult(AddressableAssetGroup assetGroup, List<AnalyzeResult> analyzeResults)
        {
            analyzeResults.Add(new AnalyzeResult()
            {
                resultName = $"'{assetGroup.name}' does not have the {nameof(BundledGroupSchema)}.  It is strongly recommended that you use it, as it will give you a lot more flexibility with asset updating.",
                severity = MessageType.Warning
            });
        }

        #endregion

        #region Fix

        public override void FixIssues(AddressableAssetSettings settings)
        {
            foreach (var assetGroupToFix in Analyze(settings))
            {
                assetGroupToFix.AddSchema<BundledGroupSchema>();

            }

            ClearAnalysis();
        }

        #endregion
    }
}
#endif