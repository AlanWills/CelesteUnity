using Celeste.Events;
using UnityEngine;

namespace Celeste.Twine.UI
{
    [CreateAssetMenu(fileName = nameof(FollowLinkUISettings), menuName = "Celeste/Twine/UI/Follow Link UI Settings")]
    public class FollowLinkUISettings : ScriptableObject
    {
        [Header("Validation")]
        public Color validColour = Color.white;
        public Color invalidColour = Color.red;

        [Header("Events")]
        public IntEvent followLink;
    }
}
