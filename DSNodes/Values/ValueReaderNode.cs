using Celeste.Parameters;
using System;
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
            TValue valueParameter = GetInputValue(nameof(value), value);
            if (valueParameter == null)
            {
                Debug.Assert(Application.isPlaying, $"Could not find value parameter for ValueReaderNode {name} in DataGraph {graph.name}.  Default value was null? {value == null}.");
                return default;
            }

            T v = GetInputValue(nameof(value), value).Value;

            if (string.CompareOrdinal(port.fieldName, nameof(output)) == 0)
            {
                return v;
            }
            
            if (string.CompareOrdinal(port.fieldName, nameof(outputAsString)) == 0 && v != null)
            {
                return v.ToString();
            }

            return default;
        }

        #endregion
    }
}
