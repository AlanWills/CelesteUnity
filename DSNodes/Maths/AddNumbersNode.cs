using Celeste.DS;
using XNode;

namespace Celeste.DSNodes
{
    [CreateNodeMenu("Celeste/Maths/Add Numbers")]
    public class AddNumbersNode : DataNode 
    {
        #region Properties and Fields

        public const string VALUE_PORT_NAME = "Value";
        public const string INCREMENT_PORT_NAME = "Increment";
        public const string OUTPUT_PORT_NAME = "Output";

        #endregion

        public AddNumbersNode()
        {
            AddDynamicInput(typeof(void), ConnectionType.Override, fieldName: VALUE_PORT_NAME);
            AddDynamicInput(typeof(void), ConnectionType.Override, fieldName: INCREMENT_PORT_NAME);
            AddDynamicOutput(typeof(void), ConnectionType.Override, fieldName: OUTPUT_PORT_NAME);
        }

        public override object GetValue(NodePort port)
        {
            return this.GetNumericInputValue(VALUE_PORT_NAME) + this.GetNumericInputValue(INCREMENT_PORT_NAME);
        }
    }
}
