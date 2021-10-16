using Celeste.Logic;
using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Logic
{
    [CustomEditor(typeof(InVector3IntArrayCondition))]
    public class InVector3IntArrayConditionEditor : ConditionEditor
    {
        protected override void OnInspectorGUIImpl(SerializedObject valueCondition)
        {
            string[] operatorDisplayNames = new string[]
            {
                "Contains",
                "Not Contains"
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
