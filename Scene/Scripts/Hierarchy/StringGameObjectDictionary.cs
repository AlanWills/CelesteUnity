using Celeste.Objects;
using UnityEngine;

namespace Celeste.Scene.Hierarchy
{
    [CreateAssetMenu(fileName = nameof(StringGameObjectDictionary), order = CelesteMenuItemConstants.SCENE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SCENE_MENU_ITEM + "String Game Object Dictionary")]
    public class StringGameObjectDictionary : DictionaryScriptableObject<string, GameObject>
    {
    }
}
