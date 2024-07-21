using System.IO;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    public enum LoadMode
    {
        Overwrite,
        RespectVersion
    }

    public abstract class Snapshot : ScriptableObject
    {
        public abstract void AddItem(string unpackPath, string snapshotData);
        public abstract void UnpackItems(LoadMode loadMode);
        public abstract string Serialize();

        protected void UnpackItem(LoadMode loadMode, string targetFilePath, string newDataSource)
        {
            if (loadMode == LoadMode.RespectVersion)
            {
                VersionedDTO existingData = default;

                if (PersistenceUtility.CanLoad(targetFilePath))
                {
                    existingData = PersistenceUtility.Load<VersionedDTO>(targetFilePath);
                }

                VersionedDTO newData = JsonUtility.FromJson<VersionedDTO>(newDataSource);

                if (existingData == null)
                {
                    UnityEngine.Debug.Log($"Unpacking snapshot data to {targetFilePath} with contents {newData}.  No previous data existed so it's safe to unpack.");
                    File.WriteAllText(targetFilePath, newDataSource);
                }
                else if (existingData.versionInformation.IsLowerVersionThan(newData))
                {
                    UnityEngine.Debug.Log($"Unpacking snapshot data to {targetFilePath} with contents {newData}.  We are respecting versioning, but the new data has a higher version or more recent timestamp than the old so it is safe to overwrite.");
                    File.WriteAllText(targetFilePath, newDataSource);
                }
                else
                {
                    UnityEngine.Debug.Log($"Not unpacking snapshot data to {targetFilePath} as we have requested to respect versioning and what is in the snapshot is lower version than what exists.");
                }
            }
            else
            {
                UnityEngine.Debug.Log($"Unpacking snapshot data to {targetFilePath}.  Ignoring versioning as we have requested to overwrite any save.");
                File.WriteAllText(targetFilePath, newDataSource);
            }
        }
    }
}
