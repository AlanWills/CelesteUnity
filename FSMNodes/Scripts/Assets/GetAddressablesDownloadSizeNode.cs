using Celeste.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using XNode;

namespace Celeste.FSM.Nodes.Assets
{
    [Serializable]
    [CreateNodeMenu("Celeste/Assets/Get Addressables Download Size")]
    public class GetAddressablesDownloadSizeNode : FSMNode
    {
        #region Properties and Fields

        public string label;

        [Output]
        public long size;

        private AsyncOperationHandle<long> downloadOperation;
        private bool complete = false;

        #endregion

        #region FSM Runtime Overrides

        protected override void OnEnter()
        {
            base.OnEnter();

            downloadOperation = Addressables.GetDownloadSizeAsync(label);
            downloadOperation.Completed += DownloadOperation_Completed; ;

            complete = false;
        }

        protected override FSMNode OnUpdate()
        {
            return complete ? base.OnUpdate() : this;
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
                    HudLog.LogInfo($"{label} download size is {size}");
                }
            }
            else
            {
                HudLog.LogError($"Failed to check download size for {label}");
            }
        }

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            return size;
        }

        #endregion

        #region Callbacks

        private void DownloadOperation_Completed(AsyncOperationHandle<long> obj)
        {
            complete = true;
            size = obj.Result;
        }

        #endregion
    }
}
