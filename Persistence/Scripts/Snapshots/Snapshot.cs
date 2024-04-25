using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    public abstract class Snapshot : ScriptableObject
    {
        public abstract void AddItem(string unpackPath, string snapshotData);
        public abstract void UnpackItems();

        public abstract string Serialize();
    }
}
