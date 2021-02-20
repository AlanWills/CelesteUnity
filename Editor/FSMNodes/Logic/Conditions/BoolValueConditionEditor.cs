using Celeste.FSM.Nodes.Logic;
using Celeste.FSM.Nodes.Logic.Conditions;
using Celeste.Logic;
using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.FSM.Nodes.Logic.Conditions
{
    [CustomEditor(typeof(BoolValueCondition))]
    public class BoolValueConditionEditor : ConditionEditor
    {
        protected override void OnInspectorGUIImpl(SerializedObject valueCondition)
        {
            string[] operatorDisplayNames = new string[]
            {
                "Equals",
                "Not Equals"
            };

            int[] operators = new int[]
            {
                (int)ConditionOperator.Equals,
                (int)ConditionOperator.NotEquals
            };

            BoolReference reference = (valueCondition.targetObject as BoolValueCondition).target;
            DrawDefaultInspectorGUI(valueCondition, operatorDisplayNames, operators, reference);
        }
    }
}
