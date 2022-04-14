using Celeste.Events;
using UnityEngine;

namespace Celeste.Twine.UI
{
    [CreateAssetMenu(fileName = nameof(TwineNodeUIControllerSettings), menuName = "Celeste/Twine/UI/Twine Node UI Controller Settings")]
    public class TwineNodeUIControllerSettings : ScriptableObject
    {
        [Header("Validation")]
        public Color validColour = Color.white;
        public Color invalidColour = Color.red;

        [Header("Events")]
        public ShowPopupEvent showEditTwineNodePopup;
    }
}
