#if USE_ADDRESSABLES
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.FSM.Nodes.Assets
{
    [Serializable]
    [CreateNodeMenu("Celeste/Assets/Update Addressables")]
    public class UpdateAddressablesNode : FSMNode
    {
        #region Properties and Fields
        
        public FloatValue progress;

        private AsyncOperationHandle<List<string>> checkForCatalogueUpdatesOperation;
        private AsyncOperationHandle<List<IResourceLocator>> updateCataloguesOperation;
        private bool complete = false;

        #endregion

        #region FSM Runtime Overrides

        protected override void OnEnter()
        {
            base.OnEnter();

            checkForCatalogueUpdatesOperation = Addressables.CheckForCatalogUpdates();
            checkForCatalogueUpdatesOperation.Completed += CheckForCatalogueUpdatesOperation_Completed;

            complete = false;

            if (progress != null)
            {
                progress.Value = 0;
            }
        }

        protected override FSMNode OnUpdate()
        {
            if (progress != null)
            {
                if (checkForCatalogueUpdatesOperation.IsValid() && !checkForCatalogueUpdatesOperation.IsDone)
                {
                    progress.Value = checkForCatalogueUpdatesOperation.PercentComplete;
                }
                else if (updateCataloguesOperation.IsValid() && !updateCataloguesOperation.IsDone)
                {
                    progress.Value = updateCataloguesOperation.PercentComplete;
                }
            }

            if (complete)
            {
                return base.OnUpdate();
            }

            return this;
        }

        #endregion

        #region Callbacks

        private void CheckForCatalogueUpdatesOperation_Completed(AsyncOperationHandle<List<string>> obj)
        {
            complete = obj.Result.Count == 0;

            if (progress != null)
            {
                progress.Value = complete ? 100 : 0;
            }

            if (!complete)
            {
                Debug.LogFormat("Downloading {0} updates", obj.Result.Count);

                updateCataloguesOperation = Addressables.UpdateCatalogs(obj.Result);
                updateCataloguesOperation.Completed += UpdateCataloguesOperation_Completed;
            }
        }

        private void UpdateCataloguesOperation_Completed(AsyncOperationHandle<List<IResourceLocator>> obj)
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
#endif