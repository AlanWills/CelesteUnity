using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
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
    }
}
