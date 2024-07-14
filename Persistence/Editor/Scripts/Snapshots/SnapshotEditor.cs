using Celeste.Persistence.Snapshots;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Persistence.Snapshots
{
    [CustomEditor(typeof(Snapshot), true, isFallback = true)]
    public class SnapshotEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Force Load"))
                {
                    (target as Snapshot).UnpackItems(LoadMode.Overwrite);
                }

                if (GUILayout.Button("Versioned Load"))
                {
                    (target as Snapshot).UnpackItems(LoadMode.RespectVersion);
                }
            }

            base.OnInspectorGUI();
        }
    }
}
