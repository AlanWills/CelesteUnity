using Celeste.Persistence.Snapshots;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Persistence
{
    public static class PersistenceMenuItems
    {
        [MenuItem("Celeste/Tools/Load Data Snapshot")]
        public static void LoadDataSnapshotFromFile()
        {
            string filePath = EditorUtility.OpenFilePanel("Select Data Snapshot", "", "*");

            if (File.Exists(filePath))
            {
                string fileContents = File.ReadAllText(filePath);
                DataSnapshot dataSnapshot = ScriptableObject.CreateInstance<DataSnapshot>();
                JsonUtility.FromJsonOverwrite(fileContents, dataSnapshot);
                dataSnapshot.UnpackItems();
            }
        }
    }
}
