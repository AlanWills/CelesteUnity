using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNode;

namespace Celeste.DS.Nodes.Conversion
{
    [Serializable]
    public abstract class ToStringNode<T> : DataNode
    {
        #region Properties and Fields

        [Input]
        public T input;

        [Input]
        public string format;

        [Output]
        public string output;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            T _input = GetInputValue("input", input);
            string _format = GetInputValue("format", format);
            return string.IsNullOrEmpty(_format) ? _input.ToString() : string.Format(_format, _input);
        }

        #endregion
    }
}
