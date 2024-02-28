using Celeste.Events;
using UnityEngine;

namespace Celeste.Twine.UI
{
    [CreateAssetMenu(fileName = nameof(TwineNodeUIControllerSettings), order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "UI/Twine Node UI Controller Settings")]
    public class TwineNodeUIControllerSettings : ScriptableObject
    {
        [Header("Validation")]
        public Color validColour = Color.white;
        public Color invalidColour = Color.red;

        [Header("Events")]
        public ShowPopupEvent showEditTwineNodePopup;
    }
}
