using Celeste.Log;
using Celeste.Parameters;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.FSM.Nodes.Assets
{
    [Serializable]
    [CreateNodeMenu("Celeste/Assets/Download Addressables")]
    public class DownloadAddressablesNode : FSMNode
    {
        #region Properties and Fields

        public string label;
        public FloatValue progress;

        private AsyncOperationHandle downloadOperation;
        private bool complete = false;

        #endregion

        #region FSM Runtime Overrides

        protected override void OnEnter()
        {
            base.OnEnter();
            
            downloadOperation = Addressables.DownloadDependenciesAsync(label);
            downloadOperation.Completed += DownloadOperation_Completed;

            complete = false;

            if (progress != null)
            {
                progress.Value = 0;
            }
        }

        protected override FSMNode OnUpdate()
        {
            if (progress != null && downloadOperation.IsValid() && !downloadOperation.IsDone)
            {
                progress.Value = downloadOperation.PercentComplete * 100;
            }

            if (complete)
            {
                return base.OnUpdate();
            }

            return this;
        }

        protected override void OnExit()
        {
            base.OnExit();

            if (downloadOperation.IsValid())
            {
                if (downloadOperation.Status == AsyncOperationStatus.Failed)
                {
                    UnityEngine.Debug.LogException(downloadOperation.OperationException, CelesteLog.Addressables);
                }
                else if (downloadOperation.Status == AsyncOperationStatus.Succeeded)
                {
                    UnityEngine.Debug.Log($"{label} downloaded correctly", CelesteLog.Addressables);
                }
            }
            else
            {
                UnityEngine.Debug.LogError($"Failed to download {label}", CelesteLog.Addressables);
            }
        }

        #endregion

        #region Callbacks

        private void DownloadOperation_Completed(AsyncOperationHandle obj)
        {
            complete = true;

            if (progress != null)
            {
                progress.Value = 100;
            }
        }

        #endregion
    }
}
