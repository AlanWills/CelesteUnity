using System;
using Celeste.Objects;
using Celeste.Tools;
using System.Collections.Generic;
using Celeste.DataStructures;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Objects
{
    public abstract class CataloguePostProcessor<TAsset, TCatalogue> : AssetPostprocessor 
        where TAsset : UnityEngine.Object
        where TCatalogue : UnityEngine.ScriptableObject, IIndexableItems<TAsset> 
    {
        protected static void HandleOnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            CompletelyRefreshAssetsInCatalogues();
        }

        private static void CompletelyRefreshAssetsInCatalogues()
        {
            HashSet<TAsset> existingItems = new HashSet<TAsset>();

            foreach (TCatalogue catalogue in EditorOnly.FindAssets<TCatalogue>())
            {
                existingItems.Clear();
                catalogue.ClearDuplicates();

                for (int i = catalogue.Items.Count - 1;  i >= 0; --i)
                {
                    TAsset item = catalogue.GetItem(i);

                    if (item != null && ShouldAddAsset(item))
                    {
                        existingItems.Add(item);
                    }
                    else
                    {
                        UnityEngine.Debug.LogAssertion($"Detected and stripped out null item from {typeof(TCatalogue).Name} {catalogue.name}.");
                        catalogue.RemoveItem(i);
                    }
                }

                if (catalogue is IAutomaticImportSettings importSettings)
                {
                    switch (importSettings.ImportBehaviour)
                    {
                        case AutomaticImportBehaviour.ImportAllAssets:
                            ImportAllAssets(catalogue, existingItems);
                            break;

                        case AutomaticImportBehaviour.ImportAssetsInCatalogueDirectoryAndSubDirectories:
                            ImportAssetsInDirectoriesAndSubDirectories(catalogue, existingItems);
                            break;

                        case AutomaticImportBehaviour.ImportAssetsInCatalogueDirectory:
                            ImportAssetsInDirectoryOnly(catalogue, existingItems);
                            break;
                        
                        case AutomaticImportBehaviour.None:
                        default:
                            break;
                    }
                }
                else
                {
                    ImportAssetsInDirectoriesAndSubDirectories(catalogue, existingItems);
                }
            }
        }

        private static void ImportAllAssets(TCatalogue catalogue, HashSet<TAsset> existingItems)
        {
            foreach (TAsset asset in EditorOnly.FindAssets<TAsset>())
            {
                if (!existingItems.Contains(asset) && ShouldAddAsset(asset))
                {
                    catalogue.AddItem(asset);
                }
            }

            catalogue.TrySyncGuids();
        }

        private static void ImportAssetsInDirectoriesAndSubDirectories(TCatalogue catalogue, HashSet<TAsset> existingItems)
        {
            string catalogueFolder = EditorOnly.GetAssetFolderPath(catalogue);
            List<TAsset> assetsToRemove = new List<TAsset>();

            foreach (TAsset asset in existingItems)
            {
                string assetFolder = EditorOnly.GetAssetFolderPath(asset);

                if (!assetFolder.StartsWith(catalogueFolder, StringComparison.Ordinal) || !ShouldAddAsset(asset))
                {
                    // This is not a valid item as it is not located in the same folder structure, so ensure it is removed
                    assetsToRemove.Add(asset);
                }
            }

            foreach (TAsset asset in assetsToRemove)
            {
                existingItems.Remove(asset);
            }

            foreach (TAsset asset in EditorOnly.FindAssets<TAsset>("", EditorOnly.GetAssetFolderPath(catalogue)))
            {
                if (!existingItems.Contains(asset) && ShouldAddAsset(asset))
                {
                    catalogue.AddItem(asset);
                }
            }
            
            catalogue.TrySyncGuids();
        }

        private static void ImportAssetsInDirectoryOnly(TCatalogue catalogue, HashSet<TAsset> existingItems)
        {
            string catalogueFolder = EditorOnly.GetAssetFolderPath(catalogue);
            List<TAsset> assetsToRemove = new List<TAsset>();

            foreach (TAsset asset in existingItems)
            {
                string assetFolder = EditorOnly.GetAssetFolderPath(asset);

                if (string.CompareOrdinal(assetFolder, catalogueFolder) != 0 || !ShouldAddAsset(asset))
                {
                    // This is not a valid item as it has a different folder path, so ensure it is removed
                    assetsToRemove.Add(asset);
                }
            }

            foreach (TAsset asset in assetsToRemove)
            {
                existingItems.Remove(asset);
            }

            foreach (TAsset asset in EditorOnly.FindAssets<TAsset>("", catalogueFolder))
            {
                string assetPath = EditorOnly.GetAssetFolderPath(asset);

                if (!existingItems.Contains(asset) && 
                    string.CompareOrdinal(assetPath, catalogueFolder) == 0 &&
                    ShouldAddAsset(asset))
                {
                    // Ensure the asset has the same asset folder path
                    catalogue.AddItem(asset);
                }
            }
            
            catalogue.TrySyncGuids();
        }

        private static bool ShouldAddAsset(TAsset asset)
        {
            if (asset is IAutomaticImportAssetSettings settings)
            {
                return settings.ImportSettings.AddToCatalogue;
            }

            return true;
        }
    }
}
