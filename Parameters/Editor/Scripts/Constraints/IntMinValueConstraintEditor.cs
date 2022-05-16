using Celeste.Parameters.Constraints;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters.Constraints
{
    [CustomEditor(typeof(IntMinValueConstraint))]
    public class IntMinValueConstraintEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Init"))
            {
                (target as IntMinValueConstraint).Init();
            }

            base.OnInspectorGUI();
        }
    }
}
