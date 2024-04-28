using Celeste.DS;
using XNode;

namespace Celeste.DSNodes
{
    [CreateNodeMenu("Celeste/Maths/Multiply Numbers")]
    public class MultiplyNumbersNode : DataNode 
    {
        #region Properties and Fields

        public const string VALUE_PORT_NAME = "Value";
        public const string MULTIPLIER_PORT_NAME = "Multiplier";

        [Output]
        public float output;

        #endregion

        public MultiplyNumbersNode()
        {
            AddDynamicInput(typeof(void), ConnectionType.Override, fieldName: VALUE_PORT_NAME);
            AddDynamicInput(typeof(void), ConnectionType.Override, fieldName: MULTIPLIER_PORT_NAME);
        }

        public override object GetValue(NodePort port)
        {
            return this.GetNumericInputValue(VALUE_PORT_NAME) * this.GetNumericInputValue(MULTIPLIER_PORT_NAME, 1.0f);
        }
    }
}
