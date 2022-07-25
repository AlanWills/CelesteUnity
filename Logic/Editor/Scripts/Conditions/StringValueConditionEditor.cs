using Celeste.Logic;
using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Logic
{
    [CustomEditor(typeof(StringValueCondition))]
    public class StringValueConditionEditor : ParameterizedValueConditionEditor
    {
        protected override void OnInspectorGUIImpl(SerializedObject valueCondition)
        {
            string[] operatorDisplayNames = new string[]
            {
                "Equals",
                "Not Equals",
            };

            int[] operators = new int[]
            {
                (int)ConditionOperator.Equals,
                (int)ConditionOperator.NotEquals,
            };

            DrawDefaultInspectorGUI(valueCondition, operatorDisplayNames, operators);
        }
    }
}
