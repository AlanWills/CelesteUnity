﻿using Celeste.Parameters;
using System;

namespace Celeste.FSM.Nodes.Parameters
{
    [Serializable]
    [CreateNodeMenu("Celeste/Parameters/Set Bool Value")]
    [NodeWidth(250)]
    public class SetBoolValueNode : SetValueNode<bool, BoolValue, BoolReference>
    {
        #region FSM Runtime

        protected override void SetValue(bool newValue)
        {
            value.Value = newValue;
        }

        #endregion
    }
}
