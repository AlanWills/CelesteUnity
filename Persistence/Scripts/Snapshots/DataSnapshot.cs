using Celeste.DataStructures;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    // A more heavyweight snapshot where all data is embedded and baked into this snapshot.
    // This is simpler to use out of the box as all data is contained within it, but could represent a larger overall size.
    // Generally though, this is the more recommended approach, especially when delivering save data over the air.
    [CreateAssetMenu(fileName = nameof(DataSnapshot), order = CelesteMenuItemConstants.PERSISTENCE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.PERSISTENCE_MENU_ITEM + "Snapshots/Data Snapshot")]
    public class DataSnapshot : Snapshot
    {
        #region Utility Classes

        [Serializable]
        private struct Data
        {
            public string UnpackPath;
            [Json] public string SourceData;
        }

        #endregion

        #region Properties and Fields

        public int NumDataFiles => data.Count;

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

        public string GetUnpackPath(int index)
        {
            return data.Get(index).UnpackPath;
        }

        public string GetData(int index)
        {
            return data.Get(index).SourceData;
        }

        public string FindData(string unpackPath)
        {
            UnityEngine.Debug.Assert(data.Exists(x => string.CompareOrdinal(x.UnpackPath, unpackPath) == 0), $"{unpackPath} has not been registered in the snapshot.");
            return data.Find(x => string.CompareOrdinal(x.UnpackPath, unpackPath) == 0).SourceData;
        }

        public TDTO DeserializeData<TDTO>(string unpackPath) where TDTO : class
        {
            string data = FindData(unpackPath);
            var deserializeResult = PersistenceUtility.Deserialize<TDTO>(data);
            return deserializeResult.Item1.Succeeded ? deserializeResult.Item2 : default;
        }

        public override void UnpackItems(LoadMode loadMode)
        {
            UnityEngine.Debug.Log($"Snapshot has {data.Count} files contained within it.");
            for (int i = 0, n = data.Count; i < n; ++i)
            {
                Data snapshotData = data[i];
                string targetFilePath = Path.Combine(Application.persistentDataPath, snapshotData.UnpackPath);

                UnpackItem(loadMode, targetFilePath, snapshotData.SourceData);
            }
        }

        public override string Serialize()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
