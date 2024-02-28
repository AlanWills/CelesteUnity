using Celeste.Objects;
using UnityEngine;

namespace Celeste.Persistence.Snapshots
{
    [CreateAssetMenu(fileName = nameof(SnapshotList), order = CelesteMenuItemConstants.PERSISTENCE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.PERSISTENCE_MENU_ITEM + "Snapshots/Snapshot List")]
    public class SnapshotList : ListScriptableObject<Snapshot>
    {
    }
}
