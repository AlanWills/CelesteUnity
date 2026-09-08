using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNode;

namespace Celeste.DS.Nodes.Conversion
{
    [CreateNodeMenu("Celeste/Conversion/Int To UInt")]
    public class IntToUIntNode : DataNode
    {
        #region Properties and Fields

        [Input] public int input;
        [Output] public uint output;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            int _input = GetInputValue("input", input);
            return (uint)_input;
        }

        #endregion
    }
}
