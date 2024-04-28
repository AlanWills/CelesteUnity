using Celeste.Objects;
using Celeste.Tools;
using System.Collections.Generic;
using UnityEditor;

namespace CelesteEditor.Objects
{
    public abstract class CataloguePostProcessor<TAsset, TCatalogue> : AssetPostprocessor 
        where TAsset : UnityEngine.Object
        where TCatalogue : ListScriptableObject<TAsset>
    {
        public static void ImportAssetsIntoCatalogues()
        {
            HashSet<TAsset> existingItems = new HashSet<TAsset>();

            foreach (TCatalogue catalogue in EditorOnly.FindAssets<TCatalogue>())
            {
                existingItems.Clear();

                for (int i = 0, n = catalogue.Items.Count;  i < n; ++i)
                {
                    TAsset item = catalogue.GetItem(i);

                    if (item != null)
                    {
                        existingItems.Add(item);
                    }
                    else
                    {
                        UnityEngine.Debug.LogAssertion($"Detected and stripped out null item from {typeof(TCatalogue).Name} {catalogue.name}.");
                        catalogue.RemoveItemAt(i);
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
                if (!existingItems.Contains(asset))
                {
                    catalogue.AddItem(asset);
                }
            }
        }

        private static void ImportAssetsInDirectoriesAndSubDirectories(TCatalogue catalogue, HashSet<TAsset> existingItems)
        {
            string catalogueFolder = EditorOnly.GetAssetFolderPath(catalogue);
            List<TAsset> assetsToRemove = new List<TAsset>();

            foreach (TAsset asset in existingItems)
            {
                string assetFolder = EditorOnly.GetAssetFolderPath(asset);

                if (!assetFolder.StartsWith(catalogueFolder))
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
                if (!existingItems.Contains(asset))
                {
                    catalogue.AddItem(asset);
                }
            }
        }

        private static void ImportAssetsInDirectoryOnly(TCatalogue catalogue, HashSet<TAsset> existingItems)
        {
            string catalogueFolder = EditorOnly.GetAssetFolderPath(catalogue);
            List<TAsset> assetsToRemove = new List<TAsset>();

            foreach (TAsset asset in existingItems)
            {
                string assetFolder = EditorOnly.GetAssetFolderPath(asset);

                if (string.CompareOrdinal(assetFolder, catalogueFolder) != 0)
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

                if (!existingItems.Contains(asset) && string.CompareOrdinal(assetPath, catalogueFolder) == 0)
                {
                    // Ensure the asset has the same asset folder path
                    catalogue.AddItem(asset);
                }
            }
        }
    }
}
