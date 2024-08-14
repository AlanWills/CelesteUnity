using Celeste.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if USE_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

namespace Celeste.FSM.Nodes.Loading
{
    [CreateNodeMenu("Celeste/Loading/Instantiate Prefab")]
    [NodeTint(0.2f, 0.2f, 0.6f)]
    public class InstantiatePrefabNode : FSMNode
    {
        #region Properties and Fields

#if USE_ADDRESSABLES
        public bool isAddressable = false;
        public string addressablePath;
#endif
        public GameObject prefab;
        public Transform parent;

#if USE_ADDRESSABLES
        private AsyncOperationHandle<GameObject> instantiateHandle;
#endif

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

#if USE_ADDRESSABLES
            if (isAddressable)
            {
                instantiateHandle = Addressables.InstantiateAsync(addressablePath, parent);
            }
            else
#endif
            {
                GameObject.Instantiate(prefab, parent);
            }
        }

        protected override FSMNode OnUpdate()
        {
#if USE_ADDRESSABLES
            if (!isAddressable || instantiateHandle.IsDone)
            {
                return base.OnUpdate();
            }
#endif

            return this;
        }

        protected override void OnExit()
        {
            base.OnExit();

#if USE_ADDRESSABLES
            if (isAddressable && instantiateHandle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogException(instantiateHandle.OperationException);
            }
#endif
        }

        #endregion
    }
}
