using Celeste.Logic;
using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Logic
{
    [CustomEditor(typeof(LongValueCondition))]
    public class LongValueConditionEditor : ConditionEditor
    {
        protected override void OnInspectorGUIImpl(SerializedObject valueCondition)
        {
            string[] operatorDisplayNames = new string[]
            {
                "Equals",
                "Not Equals",
                "Less Than",
                "Less Than Or Equal To",
                "Greater Than",
                "Greater Than Or Equal To"
            };

            int[] operators = new int[]
            {
                (int)ConditionOperator.Equals,
                (int)ConditionOperator.NotEquals,
                (int)ConditionOperator.LessThan,
                (int)ConditionOperator.LessThanOrEqualTo,
                (int)ConditionOperator.GreaterThan,
                (int)ConditionOperator.GreaterThanOrEqualTo,
            };

            DrawDefaultInspectorGUI(valueCondition, operatorDisplayNames, operators);
        }
    }
}
