using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    [CreateAssetMenu(fileName = nameof(SnapshotRecord), order = CelesteMenuItemConstants.PERSISTENCE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.PERSISTENCE_MENU_ITEM + "Snapshots/Snapshot Record")]
    public class SnapshotRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumBakedSnapshotLists => bakedSnapshotLists.Count;
        public SnapshotList RuntimeSnapshots
        {
            get
            {
                if (runtimeSnapshots == null)
                {
                    runtimeSnapshots = CreateInstance<SnapshotList>();
                    runtimeSnapshots.name = "Runtime Snapshots";
                }

                return runtimeSnapshots;
            }
        }

        [SerializeField] private List<SnapshotList> bakedSnapshotLists = new List<SnapshotList>();

        [NonSerialized] private List<IInterestedInSnapshots> interestedInSnapshots = new List<IInterestedInSnapshots>();
        [NonSerialized] private SnapshotList runtimeSnapshots;

        #endregion

        public void RegisterInterestInSnapshots(IInterestedInSnapshots interested)
        {
            if (interestedInSnapshots.Contains(interested))
            {
                UnityEngine.Debug.LogAssertion($"{interested.name} already registered interest in snapshots.");
                return;
            }

            interestedInSnapshots.Add(interested);
        }

        public void DeregisterInterestInSnapshots(IInterestedInSnapshots interested)
        {
            if (!interestedInSnapshots.Contains(interested))
            {
                UnityEngine.Debug.LogAssertion($"{interested.name} has not registered interest in snapshots.");
                return;
            }

            interestedInSnapshots.Remove(interested);
        }

        public DataSnapshot CreateDataSnapshot()
        {
            DataSnapshot snapshot = CreateInstance<DataSnapshot>();
            string dateTime = DateTime.Now.ToString().Replace('/', '_').Replace(':', '_');
            snapshot.name = $"Snapshot_{dateTime}";

            foreach (IInterestedInSnapshots interested in interestedInSnapshots)
            {
                if (interested is ISupportsDataSnapshots snapshots)
                {
                    object data = snapshots.Data;
                    string serializedData = PersistenceUtility.Serialize(data);
                    snapshot.AddItem(interested.UnpackPath, serializedData);
                }
            }

            UnityEngine.Debug.Log($"Snapshot creating with {snapshot.NumDataFiles} files.");
            return snapshot;
        }

        public SnapshotList GetSnapshotList(int index)
        {
            return bakedSnapshotLists.Get(index);
        }
    }
}
