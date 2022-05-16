using Celeste.Parameters.Constraints;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters.Constraints
{
    [CustomEditor(typeof(IntMaxValueConstraint))]
    public class IntMaxValueConstraintEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Init"))
            {
                (target as IntMaxValueConstraint).Init();
            }

            base.OnInspectorGUI();
        }
    }
}
