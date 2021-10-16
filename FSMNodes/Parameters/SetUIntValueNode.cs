using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.FSM.Nodes.Parameters
{
    [Serializable]
    [CreateNodeMenu("Celeste/Parameters/Set UInt Value")]
    [NodeWidth(250)]
    public class SetUIntValueNode : SetValueNode<uint, UIntValue, UIntReference>
    {
        #region Properties and Fields

        public SetMode setMode = SetMode.Absolute;

        #endregion

        #region FSM Runtime

        protected override void SetValue(uint newValue)
        {
            switch (setMode)
            {
                case SetMode.Absolute:
                    value.Value = newValue;
                    break;

                case SetMode.Increment:
                    value += newValue;
                    break;

                case SetMode.Decrement:
                    value.Value = value.Value < newValue ? 0 : value.Value - newValue;
                    break;

                default:
                    Debug.LogAssertionFormat("Unhandled SetMode {0}", setMode);
                    break;
            }
        }

        #endregion
    }
}
