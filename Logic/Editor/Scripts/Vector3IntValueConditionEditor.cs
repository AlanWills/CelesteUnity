using Celeste.Logic;
using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Logic
{
    [CustomEditor(typeof(Vector3IntValueCondition))]
    public class Vector3IntValueConditionEditor : ConditionEditor
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

            DrawDefaultInspectorGUI(valueCondition, operatorDisplayNames, operators);
        }
    }
}
