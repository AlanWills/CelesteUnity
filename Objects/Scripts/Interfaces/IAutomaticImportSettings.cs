namespace Celeste.Objects
{
    public enum AutomaticImportBehaviour
    { 
        ImportAssetsInCatalogueDirectoryAndSubDirectories,
        ImportAssetsInCatalogueDirectory,
        ImportAllAssets,
    }

    public interface IAutomaticImportSettings
    {
        AutomaticImportBehaviour ImportBehaviour { get; }
    }
}