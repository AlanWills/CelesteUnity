using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNode;

namespace Celeste.DS.Nodes.Logic
{
    [Serializable]
    [CreateNodeMenu("Celeste/Logic/Not")]
    public class NotNode : DataNode
    {
        #region Properties and Fields

        [Input]
        public bool input;

        [Output]
        public bool output;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            return !GetInputValue(nameof(input), input);
        }

        #endregion
    }
}
