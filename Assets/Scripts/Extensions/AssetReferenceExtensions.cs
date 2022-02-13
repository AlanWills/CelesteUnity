using UnityEngine.AddressableAssets;

namespace Celeste.Assets
{
    public static class AssetReferenceExtensions
    {
        public static bool ShouldLoad(this AssetReference assetReference)
        {
            return assetReference.Asset == null && !assetReference.OperationHandle.IsValid();
        }
    }
}