using Celeste.FSM;
#if !UNITY_WEBGL
using UnityEngine;
#endif

namespace Celeste.FSM.Nodes.Assets
{
    [CreateNodeMenu("Celeste/Assets/Delete Old Addressables")]
    public class DeleteOldAddressablesNode : FSMNode
    {
        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

#if !UNITY_WEBGL
            if (Caching.ClearCache())
            {
                Debug.Log("Deleted asset bundle cache");
            }
#endif
        }

        #endregion
    }
}
