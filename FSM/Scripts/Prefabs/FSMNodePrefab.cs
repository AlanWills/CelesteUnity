using Celeste.Objects;
using UnityEngine;

namespace Celeste.FSM.Prefabs
{
    [CreateAssetMenu(fileName = nameof(FSMNodePrefab), menuName = CelesteMenuItemConstants.FSM_MENU_ITEM + "Nodes/FSM Node Prefab", order = CelesteMenuItemConstants.FSM_MENU_ITEM_PRIORITY)]
    public class FSMNodePrefab : AssetWrapperScriptableObject<FSMNode>
    {
    }
}