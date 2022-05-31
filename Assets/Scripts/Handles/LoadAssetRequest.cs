using System;
using UnityEngine;

namespace Celeste.Assets
{
    public class LoadAssetRequest<T> : CustomYieldInstruction, ILoadRequest<T>, IDisposable where T : UnityEngine.Object
    {
        #region Properties and Fields

        public override bool keepWaiting => false;
        public T Asset => mAsset;

        private T mAsset = default;

        #endregion

        private LoadAssetRequest(T asset)
        {
            mAsset = asset;
        }

        /// <summary>
        /// Use this factory function if you already have the asset loaded in memory.
        /// The LoadAssetRequest will use the provided instance.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static LoadAssetRequest<T> FromAsset(T asset)
        {
            return new LoadAssetRequest<T>(asset);
        }

        /// <summary>
        /// Use this factory function if you wish no loading functionality to take place.
        /// This can be useful for null implementations or simulating loading issues.
        /// </summary>
        /// <returns></returns>
        public static LoadAssetRequest<T> FromNothing()
        {
            return new LoadAssetRequest<T>(default(T));
        }

        public void Dispose()
        {
            mAsset = null;
        }
    }
}
