using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Assets
{
    public class LoadAddressableRequest<T> : CustomYieldInstruction, ILoadRequest<T>, IDisposable where T : UnityEngine.Object
    {
        #region Properties and Fields

        public override bool keepWaiting
        {
            get { return mAsset == null && !(mAssetOperation.IsValid() && mAssetOperation.IsDone); }
        }

        public T Asset
        {
            get { return mAssetOperation.IsValid() ? mAssetOperation.Result : mAsset; }
        }

        private AsyncOperationHandle<T> mAssetOperation = default;
        private T mAsset = default;

        #endregion

        private LoadAddressableRequest(AsyncOperationHandle<T> assetOperation, T asset)
        {
            mAssetOperation = assetOperation;
            mAsset = asset;
        }

        /// <summary>
        /// Use this factory function if you already have the asset loaded in memory.
        /// The LoadAssetRequest will skip loading another copy of the asset and instead use the provided instance.
        /// Useful if you are simulating assets in the Editor.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static LoadAddressableRequest<T> FromAsset(T asset)
        {
            return new LoadAddressableRequest<T>(new AsyncOperationHandle<T>(), asset);
        }

        /// <summary>
        /// Use this factory function if you are loading using an operation typically from addressables.
        /// The LoadAssetRequest will wait for the operation to finish and 
        /// then you can obtain the loaded instance using the Asset property.
        /// Useful if you are testing addressables or loading assets on device.
        /// </summary>
        /// <param name="assetBundleRequest"></param>
        /// <returns></returns>
        public static LoadAddressableRequest<T> FromOperation(AsyncOperationHandle<T> assetBundleRequest)
        {
            return new LoadAddressableRequest<T>(assetBundleRequest, default(T));
        }

        /// <summary>
        /// Use this factory function if you wish no loading functionality to take place.
        /// This can be useful for null implementations or simulating loading issues.
        /// </summary>
        /// <returns></returns>
        public static LoadAddressableRequest<T> FromNothing()
        {
            return new LoadAddressableRequest<T>(new AsyncOperationHandle<T>(), default(T));
        }

        public void Dispose()
        {
            if (mAssetOperation.IsValid())
            {
                Addressables.Release(mAssetOperation);
            }
        }
    }
}
