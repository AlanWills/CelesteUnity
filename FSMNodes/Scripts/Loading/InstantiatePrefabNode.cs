using Celeste.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.FSM.Nodes.Loading
{
    [CreateNodeMenu("Celeste/Loading/Instantiate Prefab")]
    [NodeTint(0.2f, 0.2f, 0.6f)]
    public class InstantiatePrefabNode : FSMNode
    {
        #region Properties and Fields

        public bool isAddressable = false;
        public string addressablePath;
        public GameObject prefab;
        public Transform parent;

        private AsyncOperationHandle<GameObject> instantiateHandle;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            if (isAddressable)
            {
                instantiateHandle = Addressables.InstantiateAsync(addressablePath, parent);
            }
            else
            {
                GameObject.Instantiate(prefab, parent);
            }
        }

        protected override FSMNode OnUpdate()
        {
            if (!isAddressable || instantiateHandle.IsDone)
            {
                return base.OnUpdate();
            }

            return this;
        }

        protected override void OnExit()
        {
            base.OnExit();

            if (isAddressable && instantiateHandle.Status == AsyncOperationStatus.Failed)
            {
                HudLog.LogError(instantiateHandle.OperationException.Message);
            }
        }

        #endregion
    }
}
