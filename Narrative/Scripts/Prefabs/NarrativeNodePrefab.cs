using Celeste.Objects;
using UnityEngine;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = nameof(NarrativeNodePrefab), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Nodes/Narrative Node Prefab", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class NarrativeNodePrefab : AssetWrapperScriptableObject<NarrativeNode>
    {
    }
}