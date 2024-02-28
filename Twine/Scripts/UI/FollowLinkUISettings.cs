using Celeste.Events;
using UnityEngine;

namespace Celeste.Twine.UI
{
    [CreateAssetMenu(fileName = nameof(FollowLinkUISettings), order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "UI/Follow Link UI Settings")]
    public class FollowLinkUISettings : ScriptableObject
    {
        [Header("Validation")]
        public Color validColour = Color.white;
        public Color invalidColour = Color.red;

        [Header("Events")]
        public IntEvent followLink;
    }
}
