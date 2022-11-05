using System.ComponentModel;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace CelesteEditor.Assets.Schemas
{
    [DisplayName("Bundled Group")]
    public class BundledGroupSchema : AddressableAssetGroupSchema
    {
        #region Properties and Fields

        public bool BundleInStreamingAssets => bundleInStreamingAssets;

        [SerializeField] private bool bundleInStreamingAssets = true;

        #endregion
    }
}
