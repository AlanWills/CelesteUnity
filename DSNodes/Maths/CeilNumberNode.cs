using Celeste.DS;
using UnityEngine;
using XNode;

namespace Celeste.DSNodes
{
    [CreateNodeMenu("Celeste/Maths/Ceil Number")]
    public class CeilNumberNode : DataNode 
    {
        #region Properties and Fields

        public const string VALUE_PORT_NAME = "Value";
        
        [Output] public int output;
        [Output] public string outputAsString;

        #endregion

        public CeilNumberNode()
        {
            AddDynamicInput(typeof(void), ConnectionType.Override, fieldName: VALUE_PORT_NAME);
        }

        public override object GetValue(NodePort port)
        {
            float input = this.GetNumericInputValue(VALUE_PORT_NAME);
            int ceil = Mathf.CeilToInt(input);
            
            if (string.CompareOrdinal(port.fieldName, nameof(output)) == 0)
            {
                return ceil;
            }
            
            if (string.CompareOrdinal(port.fieldName, nameof(outputAsString)) == 0)
            {
                return ceil.ToString();
            }

            return null;
        }
    }
}
