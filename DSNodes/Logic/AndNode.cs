using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNode;

namespace Celeste.DS.Nodes.Logic
{
    [Serializable]
    [CreateNodeMenu("Celeste/Logic/And")]
    public class AndNode : DataNode
    {
        #region Properties and Fields

        [Input]
        public bool input1;

        [Input]
        public bool input2;

        [Output]
        public bool result;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            return GetInputValue("input1", input1) && GetInputValue("input2", input2);
        }

        #endregion
    }
}
