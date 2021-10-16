using Celeste.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNode;

namespace Celeste.DSNodes
{
    [CreateNodeMenu("Celeste/Maths/Divide")]
    public class DivideNode : DataNode 
    {
        #region Properties and Fields

        public const string NUMERATOR_PORT_NAME = "Numerator";
        public const string DENOMINATOR_PORT_NAME = "Denominator";

        [Output]
        public float output;

        #endregion

        public DivideNode()
        {
            AddDynamicInput(typeof(void), ConnectionType.Override, fieldName: NUMERATOR_PORT_NAME);
            AddDynamicInput(typeof(void), ConnectionType.Override, fieldName: DENOMINATOR_PORT_NAME);
        }

        public override object GetValue(NodePort port)
        {
            return this.GetNumericInputValue(NUMERATOR_PORT_NAME) / this.GetNumericInputValue(DENOMINATOR_PORT_NAME, 1.0f);
        }
    }
}
