using Celeste.Parameters;
using System;
using XNode;

namespace Celeste.DS.Nodes.Values
{
    [Serializable]
    public abstract class ValueReaderNode<T, TValue> : DataNode where TValue : IValue<T>
    {
        #region Properties and Fields

        [Input]
        public TValue value;

        [Output]
        public T output;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            return GetInputValue(nameof(value), value).Value;
        }

        #endregion
    }
}
