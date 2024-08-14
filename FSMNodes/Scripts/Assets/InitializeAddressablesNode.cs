#if USE_ADDRESSABLES
using Celeste.Assets;
using Celeste.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.FSM.Nodes.Assets
{
    [CreateNodeMenu("Celeste/Assets/Initialize Addressables")]
    public class InitializeAddressablesNode : FSMNode
    {
        #region Properties and Fields

        private AsyncOperationHandle<IResourceLocator> operationHandle;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

#if !UNITY_EDITOR
            // Fix for bug: https://forum.unity.com/threads/addressables-not-loading-in-build.925982/
            PlayerPrefs.DeleteKey(Addressables.kAddressablesRuntimeDataPath);
#endif

            operationHandle = Addressables.InitializeAsync();
        }

        protected override FSMNode OnUpdate()
        {
            return operationHandle.IsDone ? base.OnUpdate() : this;
        }

        protected override void OnExit()
        {
            base.OnExit();

            if (operationHandle.IsValid() && operationHandle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogErrorFormat("Failed to initialize Addressables: {0}", 
                    operationHandle.OperationException != null ? operationHandle.OperationException.Message : "No exception found");
            }
        }

        #endregion
    }
}
#endif