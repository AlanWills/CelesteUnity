using UnityEngine;

namespace Celeste.Objects.Types
{
    [CreateAssetMenu(fileName = nameof(StringList), menuName = CelesteMenuItemConstants.OBJECTS_MENU_ITEM + "Lists/String List", order = CelesteMenuItemConstants.OBJECTS_MENU_ITEM_PRIORITY)]
    public class StringList : ListScriptableObject<string>
    {
    }
}
