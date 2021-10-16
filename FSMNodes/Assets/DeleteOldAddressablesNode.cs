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

namespace Celeste.FSMNodes.Assets
{
    [CreateNodeMenu("Celeste/Assets/Delete Old Addressables Node")]
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
