#if USE_ADDRESSABLES
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Assets.AssetReferences
{
    public class CelesteAssetReference<T> : AssetReferenceT<T> where T : Object
    {
        #region Properties and Fields

        public bool ShouldLoad => this.ShouldLoad();
        public new T Asset => base.Asset as T;

        #endregion

        public CelesteAssetReference(string guid) : base(guid)
        {
        }

        public new AsyncOperationHandle<T> LoadAssetAsync()
        {
            if (!ShouldLoad)
            {
                UnityEngine.Debug.Assert(OperationHandle.IsValid() && OperationHandle.IsDone, $"An asset was marked as not needing loading, but its operation handle is not valid.");
                return OperationHandle.Convert<T>();
            }

            return base.LoadAssetAsync<T>();
        }
    }
}
#endif