using Celeste.Logic;
using Celeste.Logic.Catalogue;
using CelesteEditor.Tools;
using System.Collections.Generic;
using UnityEditor;

namespace CelesteEditor.Logic.Assets
{
    public class ConditionCataloguePostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            List<Condition> conditions = AssetUtility.FindAssets<Condition>();

            foreach (ConditionCatalogue conditionCatalogue in AssetUtility.FindAssets<ConditionCatalogue>())
            {
                conditionCatalogue.SetItems(conditions);
            }
        }
    }
}
