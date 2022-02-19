using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Assets
{
    public struct AssetLoadingScope : IDisposable
    {
        private AssetLoadingHandle handle;

        public AssetLoadingScope(AssetLoadingHandle handle)
        {
            this.handle = handle;
            ++handle.LoadAssetsCount;
        }

        public void Dispose()
        {
            --handle.LoadAssetsCount;
        }
    }

    public class AssetLoadingHandle
    {
        public int LoadAssetsCount { get; set; }

        public AssetLoadingScope Use()
        {
            return new AssetLoadingScope(this);
        }
    }
}