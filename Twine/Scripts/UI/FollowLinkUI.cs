using Celeste.Events;
using PolyAndCode.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Twine.UI
{
    [AddComponentMenu("Celeste/Twine/Follow Link UI")]
    [RequireComponent(typeof(RectTransform))]
    public class FollowLinkUI : MonoBehaviour, ICell
    {
        #region Properties and Fields

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI linkName;
        [SerializeField] private Image backgroundImage;

        [Header("Data")]
        [SerializeField] private FollowLinkUISettings settings;

        private TwineNodeLink link;

        #endregion

        public void ConfigureCell(TwineNodeLink twineNodeLink, int cellIndex)
        {
            link = twineNodeLink;
            linkName.text = twineNodeLink.name;
            backgroundImage.color = twineNodeLink.IsValid ? settings.validColour : settings.invalidColour;
        }

        #region Callbacks

        public void OnButtonPressed()
        {
            settings.followLink.Invoke(link.pid);
        }

        #endregion
    }
}
