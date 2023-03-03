using UnityEngine;
using XNode;

namespace Celeste.DS
{
    [AddComponentMenu("Celeste/DS/Data Runtime")]
    public class DataRuntime : SceneGraph<DataGraph>
    {
        #region Unity Methods

        private void Start()
        {
            Debug.AssertFormat(graph != null, "{0} has a DataRuntime with no graph set", gameObject.name);
            graph.Initialize(gameObject);
        }

        private void Update()
        {
            graph.Update();
        }

        #endregion
    }
}
