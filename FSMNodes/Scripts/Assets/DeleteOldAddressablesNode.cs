using Celeste.FSM;
using UnityEngine;

namespace Celeste.FSMNodes.Assets
{
    [CreateNodeMenu("Celeste/Assets/Delete Old Addressables")]
    public class DeleteOldAddressablesNode : FSMNode
    {
        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            if (Caching.ClearCache())
            {
                Debug.Log("Deleted asset bundle cache");
            }
        }

        #endregion
    }
}
