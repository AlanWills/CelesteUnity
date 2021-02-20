using Celeste.FSM.Nodes.Logic;
using Celeste.FSM.Nodes.Logic.Conditions;
using Celeste.Logic;
using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.FSM.Nodes.Logic.Conditions
{
    [CustomEditor(typeof(StringValueCondition))]
    public class StringValueConditionEditor : ConditionEditor
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

            StringReference reference = (valueCondition.targetObject as StringValueCondition).target;
            DrawDefaultInspectorGUI(valueCondition, operatorDisplayNames, operators, reference);
        }
    }
}
