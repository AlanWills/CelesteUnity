using UnityEngine;

namespace Celeste.Objects.Types
{
    [CreateAssetMenu(fileName = nameof(IntList), menuName = CelesteMenuItemConstants.OBJECTS_MENU_ITEM + "Lists/Int List", order = CelesteMenuItemConstants.OBJECTS_MENU_ITEM_PRIORITY)]
    public class IntList : ListScriptableObject<string>
    {
    }
}
