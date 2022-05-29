using Celeste.LiveOps;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.LiveOps
{
    [CustomEditor(typeof(LiveOpsSchedule))]
    public class LiveOpsScheduleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Build From Templates"))
            {
                (target as LiveOpsSchedule).EditorOnly_BuildFromTemplates();
            }

            base.OnInspectorGUI();
        }
    }
}
