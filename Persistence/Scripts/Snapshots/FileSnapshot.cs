using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    // A lightweight snapshot which does not embed the data, but rather references it in a file system.
    // When unpacking, it will find the appropriate file and use it's contents.
    // This can be useful when baking save files into a game - the individual files can be organised in the file system
    // and then this snapshot will reference their locations and write their data at runtime.
    // Different snapshots can also reference the same file, allowing efficiencies with overall snapshot file sizes.
    [CreateAssetMenu(fileName = nameof(FileSnapshot), menuName = "Celeste/Persistence/Snapshots/File Snapshot")]
    public class FileSnapshot : Snapshot
    {
        #region Utility Classes

        [Serializable]
        private struct Data
        {
            public string UnpackPath;
            public string SourceFilePath;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private List<Data> data = new List<Data>();

        #endregion

        public override void AddItem(string unpackPath, string sourceFilePath)
        {
            UnityEngine.Debug.Assert(!data.Exists(x => string.CompareOrdinal(x.UnpackPath, unpackPath) == 0), $"{unpackPath} has already been registered in the snapshot.  This may lead to data overriding when the snapshot is unpacked.");
            data.Add(new Data()
            {
                UnpackPath = unpackPath,
                SourceFilePath = sourceFilePath,
            });
        }

        public override void UnpackItems()
        {
            for (int i = 0, n = data.Count; i < n; ++i)
            {
                Data snapshotData = data[i];
                string sourceData = File.ReadAllText(snapshotData.SourceFilePath);
                string filePath = Path.Combine(Application.persistentDataPath, snapshotData.UnpackPath);
                File.WriteAllText(filePath, sourceData);
            }
        }

        public override string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
