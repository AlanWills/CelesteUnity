using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Celeste.FSM.Nodes.Objects
{
    [Serializable]
    [CreateNodeMenu("Celeste/Objects/Execute Function")]
    public class ExecuteFunctionNode : FSMNode
    {
        #region Properties and Fields

        public UnityEvent function;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            function.Invoke();
        }

        #endregion
    }
}
