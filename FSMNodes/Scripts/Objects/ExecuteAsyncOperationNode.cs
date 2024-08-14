#if USE_ADDRESSABLES
using Celeste.Assets;
using Celeste.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Celeste.FSM.Nodes.Objects
{
    [Serializable]
    public class AsyncOperationHandleWrapperUnityEvent : UnityEvent<string, AsyncOperationHandleWrapper> { }

    [Serializable]
    [CreateNodeMenu("Celeste/Objects/Execute Async Operation")]
    public class ExecuteAsyncOperationNode : FSMNode
    {
        #region Properties and Fields

        public string address;
        public AsyncOperationHandleWrapperUnityEvent function;

        private AsyncOperationHandleWrapper handleWrapper = new AsyncOperationHandleWrapper();

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            function.Invoke(address, handleWrapper);
        }

        protected override FSMNode OnUpdate()
        {
            return handleWrapper.IsDone ? base.OnUpdate() : this;
        }

        protected override void OnExit()
        {
            base.OnExit();

            if (handleWrapper.HasError)
            {
                UnityEngine.Debug.LogError("Async Operation failed");
            }
        }

        #endregion
    }
}
#endif