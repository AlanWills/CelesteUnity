using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Loading/Scene Unloader")]
    [NodeWidth(250), NodeTint(0.2f, 0.2f, 0.6f)]
    public class SceneUnloaderNode : FSMNode
    {
        #region Properties and Fields

        public StringReference sceneName;

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

            SceneUnloaderNode originalSceneUnloader = original as SceneUnloaderNode;
            sceneName = CreateParameter(originalSceneUnloader.sceneName);
        }

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            SceneManager.UnloadSceneAsync(sceneName.Value);
        }

        #endregion
    }
}
