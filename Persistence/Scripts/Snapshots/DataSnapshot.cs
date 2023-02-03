using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    // A more heavyweight snapshot where all data is embedded and baked into this snapshot.
    // This is simpler to use out of the box as all data is contained within it, but could represent a larger overall size.
    // Generally though, this is the more recommended approach, especially when delivering save data over the air.
    [CreateAssetMenu(fileName = nameof(DataSnapshot), menuName = "Celeste/Persistence/Snapshots/Data Snapshot")]
    public class DataSnapshot : Snapshot
    {
        #region Utility Classes

        [Serializable]
        private struct Data
        {
            public string UnpackPath;
            public string SourceData;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private List<Data> data = new List<Data>();

        #endregion

        public override void AddItem(string unpackPath, string saveData)
        {
            UnityEngine.Debug.Assert(!data.Exists(x => string.CompareOrdinal(x.UnpackPath, unpackPath) == 0), $"{unpackPath} has already been registered in the snapshot.  This may lead to data overriding when the snapshot is unpacked.");
            data.Add(new Data()
            {
                UnpackPath = unpackPath,
                SourceData = saveData,
            });
        }

        public override void UnpackItems()
        {
            for (int i = 0, n = data.Count; i < n; ++i)
            {
                Data snapshotData = data[i];
                string filePath = Path.Combine(Application.persistentDataPath, snapshotData.UnpackPath);
                UnityEngine.Debug.Log($"Unpacking snapshot data {i} to {filePath} with contents {snapshotData.SourceData}.");
                File.WriteAllText(filePath, snapshotData.SourceData);
            }
        }

        public override string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
