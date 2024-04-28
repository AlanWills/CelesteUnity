using Celeste.DS;
using XNode;

namespace Celeste.DSNodes
{
    [CreateNodeMenu("Celeste/Maths/Subtract Numbers")]
    public class SubtractNumbersNode : DataNode 
    {
        #region Properties and Fields

        public const string VALUE_PORT_NAME = "Value";
        public const string DECREMENT_PORT_NAME = "Decrement";
        public const string OUTPUT_PORT_NAME = "Output";

        #endregion

        public SubtractNumbersNode()
        {
            AddDynamicInput(typeof(void), ConnectionType.Override, fieldName: VALUE_PORT_NAME);
            AddDynamicInput(typeof(void), ConnectionType.Override, fieldName: DECREMENT_PORT_NAME);
            AddDynamicOutput(typeof(void), ConnectionType.Override, fieldName: OUTPUT_PORT_NAME);
        }

        public override object GetValue(NodePort port)
        {
            return this.GetNumericInputValue(VALUE_PORT_NAME) - this.GetNumericInputValue(DECREMENT_PORT_NAME);
        }
    }
}
