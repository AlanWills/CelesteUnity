using Celeste.Parameters;
using System;
using Celeste.Tools.Attributes.GUI;
using UnityEditor;
using UnityEngine;
using XNode;

namespace Celeste.DS.Nodes.Values
{
    [Serializable]
    public abstract class ValueReaderNode<T, TValue> : DataNode where TValue : IValue<T>
    {
        #region Properties and Fields

        [Input] public TValue value;
        [Output] public T output;
        [Output] public T outputAsString;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            T v = GetInputValue(nameof(value), value).Value;

            if (string.CompareOrdinal(port.fieldName, nameof(output)) == 0)
            {
                return v;
            }
            
            if (string.CompareOrdinal(port.fieldName, nameof(outputAsString)) == 0)
            {
                return v != null ? v.ToString() : null;
            }

            return default;
        }

        #endregion
    }
}
