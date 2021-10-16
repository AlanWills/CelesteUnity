using Celeste.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNode;

namespace Celeste.DSNodes
{
    [CreateNodeMenu("Celeste/Constants/Int")]
    public class IntNode : DataNode
    {
        #region Properties and Fields

        [Output]
        public int output;

        #endregion

        public override object GetValue(NodePort port)
        {
            return output;
        }
    }
}
