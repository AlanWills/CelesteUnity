using Celeste.Objects;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    [CreateAssetMenu(fileName = nameof(SnapshotList), menuName = "Celeste/Persistence/Snapshots/Snapshot List")]
    public class SnapshotList : ListScriptableObject<Snapshot>
    {
    }
}
