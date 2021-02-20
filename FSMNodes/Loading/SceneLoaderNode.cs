using Celeste.Log;
using Celeste.Objects;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
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
        public bool isAddressable = false;

        private AsyncOperation loadOperation;
        private AsyncOperationHandle<SceneInstance> addressablesOperation;

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

            if (isAddressable)
            {
                addressablesOperation = Addressables.LoadSceneAsync(_sceneName, loadMode);
            }
            else
            {
                loadOperation = SceneManager.LoadSceneAsync(_sceneName, loadMode);
            }
        }

        protected override FSMNode OnUpdate()
        {
            if (isAddressable)
            {
                return addressablesOperation.IsDone ? base.OnUpdate() : this;
            }
            else
            {
                return loadOperation.isDone ? base.OnUpdate() : this;
            }
        }

        protected override void OnExit()
        {
            base.OnExit();

            if (isAddressable && addressablesOperation.Status == AsyncOperationStatus.Failed)
            {
                HudLog.LogError(addressablesOperation.OperationException.Message);
            }
        }

        #endregion
    }
}