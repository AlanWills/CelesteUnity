using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Assets
{
    public class AddressableOperationHandleWrapper : IDisposable
    {
        #region Properties and Fields

        public bool IsDone
        {
            get { return !handle.IsValid() || handle.IsDone || handle.Status != AsyncOperationStatus.None; }
        }

        public bool HasError
        {
            get { return !handle.IsValid() || handle.Status == AsyncOperationStatus.Failed; }
        }

        public AsyncOperationHandle handle;

        #endregion

        public AddressableOperationHandleWrapper() { }

        public AddressableOperationHandleWrapper(AsyncOperationHandle asyncOperationHandle)
        {
            handle = asyncOperationHandle;
        }

        public T Get<T>() where T : UnityEngine.Object
        {
            return handle.Result as T;
        }

        public void Dispose()
        {
            Addressables.Release(handle);
        }
    }

    public class AddressableOperationHandleWrapper<T> : IDisposable
    {
        #region Properties and Fields

        public bool IsDone
        {
            get { return !handle.IsValid() || handle.IsDone || handle.Status != AsyncOperationStatus.None; }
        }

        public bool HasError
        {
            get { return !handle.IsValid() || handle.Status == AsyncOperationStatus.Failed; }
        }

        public AsyncOperationHandle<T> handle;

        #endregion

        public AddressableOperationHandleWrapper() { }

        public AddressableOperationHandleWrapper(AsyncOperationHandle<T> asyncOperationHandle)
        {
            handle = asyncOperationHandle;
        }

        public T Get()
        {
            return handle.Result;
        }

        public void Dispose()
        {
            Addressables.Release(handle);
        }
    }
}