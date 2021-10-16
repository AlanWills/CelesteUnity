using Celeste.Log;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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
                    HudLog.LogError(downloadOperation.OperationException.Message);
                }
                else if (downloadOperation.Status == AsyncOperationStatus.Succeeded)
                {
                    HudLog.LogInfo($"{label} downloaded correctly");
                }
            }
            else
            {
                HudLog.LogError($"Failed to download {label}");
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
