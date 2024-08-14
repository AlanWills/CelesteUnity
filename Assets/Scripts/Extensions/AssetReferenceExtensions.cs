#if USE_ADDRESSABLES
using UnityEngine.AddressableAssets;

namespace Celeste.Assets
{
    public static class AssetReferenceExtensions
    {
        public static bool ShouldLoad(this AssetReference assetReference)
        {
            return !assetReference.IsValid() || assetReference.Asset == null;
        }
    }
}
#endif