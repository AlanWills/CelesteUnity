using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.FSM.Nodes.Parameters
{
    [Serializable]
    [CreateNodeMenu("Celeste/Parameters/Set Int Value")]
    [NodeWidth(250)]
    public class SetIntValueNode : SetValueNode<int, IntValue, IntReference>
    {
        #region Properties and Fields

        public SetMode setMode = SetMode.Absolute;

        #endregion

        #region FSM Runtime

        protected override void SetValue(int _newValue)
        {
            switch (setMode)
            {
                case SetMode.Absolute:
                    value.Value = _newValue;
                    break;

                case SetMode.Increment:
                    value += _newValue;
                    break;

                case SetMode.Decrement:
                    value -= _newValue;
                    break;

                default:
                    Debug.LogAssertion($"Unhandled SetMode {setMode}");
                    break;
            }
        }

        #endregion
    }
}
