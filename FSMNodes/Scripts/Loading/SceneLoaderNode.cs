using Celeste.Log;
using Celeste.Objects;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if USE_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
#endif
using UnityEngine.SceneManagement;
using XNode;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Loading/Scene Loader")]
    [NodeWidth(250), NodeTint(0.2f, 0.2f, 0.6f)]
    public class SceneLoaderNode : FSMNode
    {
        #region Properties and Fields

        [Input]
        public StringReference sceneName;
        public LoadSceneMode loadMode = LoadSceneMode.Single;
#if USE_ADDRESSABLES
        public bool isAddressable = false;
#endif

        private AsyncOperation loadOperation;
#if USE_ADDRESSABLES
        private AsyncOperationHandle<SceneInstance> addressablesOperation;
#endif

        #endregion

        #region Add/Remove/Copy

        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();

            if (sceneName == null)
            {
                sceneName = CreateParameter<StringReference>(name + "_sceneName");
            }
        }

        protected override void OnRemoveFromGraph()
        {
            base.OnRemoveFromGraph();

            RemoveParameter(sceneName);
        }

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            SceneLoaderNode originalSceneLoader = original as SceneLoaderNode;
            sceneName = CreateParameter(originalSceneLoader.sceneName);
        }

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            string _sceneName = GetInputValue<string>(nameof(sceneName), sceneName.Value);

#if USE_ADDRESSABLES
            if (isAddressable)
            {
                addressablesOperation = Addressables.LoadSceneAsync(_sceneName, loadMode);
            }
            else
#endif
            {
                loadOperation = SceneManager.LoadSceneAsync(_sceneName, loadMode);
            }
        }

        protected override FSMNode OnUpdate()
        {
#if USE_ADDRESSABLES
            if (isAddressable)
            {
                return addressablesOperation.IsDone ? base.OnUpdate() : this;
            }
            else
#endif
            {
                return loadOperation.isDone ? base.OnUpdate() : this;
            }
        }

        protected override void OnExit()
        {
            base.OnExit();

#if USE_ADDRESSABLES
            if (isAddressable && addressablesOperation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError(addressablesOperation.OperationException.Message);
            }
#endif
        }

#endregion
    }
}