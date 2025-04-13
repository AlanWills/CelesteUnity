namespace Celeste.Objects
{
    public enum AutomaticImportBehaviour
    { 
        ImportAssetsInCatalogueDirectoryAndSubDirectories = 0,
        ImportAssetsInCatalogueDirectory = 1,
        ImportAllAssets = 2,
        None = 3,
    }

    public interface IAutomaticImportSettings
    {
        AutomaticImportBehaviour ImportBehaviour { get; }
    }
}