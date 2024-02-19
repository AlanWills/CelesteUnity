﻿using Celeste.Logic;
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
            HashSet<Condition> existingItems = new HashSet<Condition>();

            foreach (ConditionCatalogue conditionCatalogue in AssetUtility.FindAssets<ConditionCatalogue>())
            {
                existingItems.Clear();

                for (int i = 0, n =  conditionCatalogue.Items.Count;  i < n; ++i)
                {
                    Condition item = conditionCatalogue.GetItem(i);

                    if (item != null)
                    {
                        existingItems.Add(item);
                    }
                    else
                    {
                        UnityEngine.Debug.LogAssertion($"Detected and stripped out null item from {nameof(ConditionCatalogue)} {conditionCatalogue.name}.");
                        conditionCatalogue.RemoveItemAt(i);
                    }
                }

                foreach (Condition condition in AssetUtility.FindAssets<Condition>("", AssetUtility.GetAssetFolderPath(conditionCatalogue)))
                {
                    if (!existingItems.Contains(condition))
                    {
                        conditionCatalogue.AddItem(condition);
                    }
                }
            }
        }
    }
}
