using Celeste.Events;
using PolyAndCode.UI;
using TMPro;
using UnityEngine;

namespace Celeste.Twine.UI
{
    [AddComponentMenu("Celeste/Twine/Follow Link UI")]
    [RequireComponent(typeof(RectTransform))]
    public class FollowLinkUI : MonoBehaviour, ICell
    {
        #region Properties and Fields

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI linkName;

        [Header("Events")]
        [SerializeField] private IntEvent followLink;

        private TwineNodeLink link;

        #endregion

        public void ConfigureCell(TwineNodeLink twineNodeLink, int cellIndex)
        {
            link = twineNodeLink;
            linkName.text = twineNodeLink.name;
        }

        #region Callbacks

        public void OnButtonPressed()
        {
            followLink.Invoke(link.pid);
        }

        #endregion
    }
}
