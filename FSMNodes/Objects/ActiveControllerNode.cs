using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.FSM.Nodes.Objects
{
    [Serializable]
    [CreateNodeMenu("Celeste/Objects/Active Controller")]
    public class ActiveControllerNode : FSMNode
    {
        #region Properties and Fields

        [Input]
        public bool isActive = false;

        public GameObject gameObject;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            bool _isActive = GetInputValue(nameof(isActive), isActive);
            gameObject.SetActive(_isActive);
        }

        #endregion
    }
}
