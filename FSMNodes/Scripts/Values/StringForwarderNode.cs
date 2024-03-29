using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Values/String Forwarder")]
    public class StringForwarderNode : FSMNode
    {
        #region Forwarder Value

        [Serializable]
        public struct ForwarderValue
        {
            public string input;
            public StringValue inputValue;
            public string output;
            public StringValue outputValue;
        }

        #endregion

        #region Properties and Fields

        [Input, SerializeField] private string inputValue;
        [Input, SerializeField] private string outputValue;

        [SerializeField] private List<ForwarderValue> forwardedValues = new List<ForwarderValue>();

        #endregion

        #region FSM Runtime

        public override object GetValue(NodePort port)
        {
            object inputValue = GetInputValue<object>(nameof(inputValue), null);

            string inputStringValue =
                inputValue is string rawString ? rawString :
                inputValue is StringValue stringValue ? stringValue.Value :
                inputValue is StringReference stringReference ? stringReference.Value : string.Empty;

            int foundIndex = forwardedValues.FindIndex(x =>
                string.CompareOrdinal(x.input, inputStringValue) == 0 ||
                (x.inputValue != null && string.CompareOrdinal(x.inputValue.Value, inputStringValue) == 0));

            var forwardedValue = forwardedValues[foundIndex];
            return forwardedValue.outputValue != null ? forwardedValue.outputValue.Value : forwardedValue.output;
        }

        #endregion
    }
}
