using System.IO;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    [AddComponentMenu("Celeste/Persistence/Snapshots/Snapshot Manager")]
    public class SnapshotManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private SnapshotRecord snapshotRecord;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Add any snapshots in persistent data
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);

            foreach (FileInfo snapshotFile in directoryInfo.EnumerateFiles(".datasnapshot", SearchOption.AllDirectories))
            {
                string snapshotText = File.ReadAllText(snapshotFile.FullName);
                var deserializeResult = PersistenceUtility.Deserialize<DataSnapshot>(snapshotText);
                UnityEngine.Debug.Assert(deserializeResult.Item1.Succeeded, $"Failed to deserialize data snapshot {snapshotFile.Name}.");

                if (!deserializeResult.Item1.Failed)
                {
                    snapshotRecord.RuntimeSnapshots.AddItem(deserializeResult.Item2);
                }
            }
        }

        #endregion
    }
}
