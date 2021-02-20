using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.Assets
{
    [Serializable]
    public class AsyncOperationHandleWrapper
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

        public AsyncOperationHandleWrapper() { }

        public AsyncOperationHandleWrapper(AsyncOperationHandle asyncOperationHandle)
        {
            handle = asyncOperationHandle;
        }

        public T Get<T>() where T : UnityEngine.Object
        {
            return handle.Result as T;
        }
    }
}