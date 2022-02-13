using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Celeste.Assets.AssetReferences
{
    public class CelesteAssetReference<T> : AssetReferenceT<T> where T : Object
    {
        public bool ShouldLoad => this.ShouldLoad();
        public new T Asset => base.Asset as T;

        public CelesteAssetReference(string guid) : base(guid)
        {
        }
    }
}