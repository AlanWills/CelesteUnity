using Celeste.Scene;
using CelesteEditor.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.AnalyzeRules;
using UnityEditor.AddressableAssets.Settings;

namespace CelesteEditor.Scene.Analyze
{
    [InitializeOnLoad]
    public static class RegisterEnsureAddressableScenesSetUpCorrectlyAnalyzeRule
    {
        static RegisterEnsureAddressableScenesSetUpCorrectlyAnalyzeRule()
        {
            AnalyzeSystem.RegisterNewRule<EnsureAddressableScenesSetUpCorrectlyAnalyzeRule>();
        }
    }

    public class EnsureAddressableScenesSetUpCorrectlyAnalyzeRule : AnalyzeRule
    {
        #region Properties and Fields

        public override string ruleName => "Ensure Addressable Scenes Set Up Correctly";
        public override bool CanFix { get; set; } = true;

        #endregion

        #region Analyze

        public override List<AnalyzeResult> RefreshAnalysis(AddressableAssetSettings settings)
        {
            Dictionary<string, SceneAsset> sceneLookup = SceneUtility.CreateSceneAssetLookup();
            List<AnalyzeResult> analyzeResults = new List<AnalyzeResult>();

            foreach (SceneAsset scene in Analyze(settings, sceneLookup))
            {
                AddAnalyzeResult(scene, analyzeResults);
            }

            CanFix = true;

            return analyzeResults;
        }

        private List<SceneAsset> Analyze(AddressableAssetSettings settings, Dictionary<string, SceneAsset> sceneLookup)
        {
            List<SceneAsset> scenesToFix = new List<SceneAsset>();

            foreach (SceneSet sceneSet in AssetUtility.FindAssets<SceneSet>())
            {
                for (int i = 0, n = sceneSet.NumScenes; i < n; ++i)
                {
                    if (sceneSet.GetSceneType(i) == SceneType.Addressable)
                    {
                        if (sceneLookup.TryGetValue(sceneSet.GetSceneId(i), out SceneAsset scene))
                        {
                            var addressableInfo = AddressablesUtility.GetAddressableInfo(scene);

                            if (addressableInfo == null || string.CompareOrdinal(addressableInfo.address, scene.name) != 0)
                            {
                                // Either not an addressable or the address is not the name, so it's incorrect
                                scenesToFix.Add(scene);
                            }
                        }
                        else
                        {
                            UnityEngine.Debug.LogAssertion($"Could not find scene {sceneSet.GetSceneId(i)} in the project, but it is included in {nameof(SceneSet)} '{sceneSet.name}'.");
                        }
                    }
                }
            }

            return scenesToFix;
        }

        private void AddAnalyzeResult(SceneAsset scene, List<AnalyzeResult> analyzeResults)
        {
            analyzeResults.Add(new AnalyzeResult()
            {
                resultName = $"'{scene.name}' is either not included in addressables or not given the correct address.",
                severity = MessageType.Error
            });
        }

        #endregion

        #region Fix

        public override void FixIssues(AddressableAssetSettings settings)
        {
            Dictionary<string, SceneAsset> sceneLookup = SceneUtility.CreateSceneAssetLookup();

            foreach (var sceneToFix in Analyze(settings, sceneLookup))
            {
                // Go through each scene and set the address to be the name
                var existingInfo = sceneToFix.GetAddressableInfo();
                sceneToFix.SetAddressableInfo(existingInfo != null ? existingInfo.parentGroup.Name : settings.DefaultGroup.Name, sceneToFix.name);
            }

            ClearAnalysis();
        }

        #endregion
    }
}