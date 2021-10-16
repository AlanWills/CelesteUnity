using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNode;

namespace Celeste.DS.Nodes.Forwarders
{
    [Serializable]
    public abstract class ValueForwarderNode<T> : DataNode
    {
        #region Properties and Fields

        [Input]
        public T defaultValue;

        [Input]
        public T forwardValue;

        [Input]
        public bool condition;

        [Output]
        public T value;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            bool _condition = GetInputValue(nameof(condition), condition);
            value = _condition ? forwardValue : defaultValue;
            return value;
        }

        #endregion
    }
}
