using Celeste.DataStructures;
using Celeste.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    [CreateAssetMenu(fileName = nameof(SnapshotRecord), menuName = "Celeste/Persistence/Snapshots/Snapshot Record")]
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
            UnityEngine.Debug.Assert(!interestedInSnapshots.Contains(interested), $"{interested.name} already registered interest in snapshots.");
            interestedInSnapshots.Add(interested);
        }

        public void DeregisterInterestInSnapshots(IInterestedInSnapshots interested)
        {
            UnityEngine.Debug.Assert(interestedInSnapshots.Contains(interested), $"{interested.name} has not registered interest in snapshots.");
            interestedInSnapshots.Remove(interested);
        }

        public DataSnapshot CreateDataSnapshot()
        {
            DataSnapshot snapshot = CreateInstance<DataSnapshot>();
            string dateTime = DateTime.Now.ToString().Replace('/', '_').Replace(':', '_');
            snapshot.name = $"Snapshot_{dateTime}";

            foreach (IInterestedInSnapshots interested in interestedInSnapshots)
            {
                if (interested is ISupportsDataSnapshots)
                {
                    object data = (interested as ISupportsDataSnapshots).Data;
                    string serializedData = PersistenceUtility.Serialize(data);
                    snapshot.AddItem(interested.UnpackPath, serializedData);
                }
            }

            return snapshot;
        }

        public SnapshotList GetSnapshotList(int index)
        {
            return bakedSnapshotLists.Get(index);
        }
    }
}
