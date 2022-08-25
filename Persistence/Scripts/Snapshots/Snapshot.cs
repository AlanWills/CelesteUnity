using Celeste.OdinSerializer;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    public abstract class Snapshot : SerializedScriptableObject
    {
        public abstract void AddItem(string unpackPath, string snapshotData);
        public abstract void UnpackItems();
    }
}
