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
            if (GUILayout.Button("Load"))
            {
                (target as Snapshot).UnpackItems();
            }

            base.OnInspectorGUI();
        }
    }
}
