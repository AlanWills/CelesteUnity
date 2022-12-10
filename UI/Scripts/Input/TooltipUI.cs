using Celeste.Events;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Input
{
    [AddComponentMenu("Celeste/UI/Input/Tooltip UI")]
    public class TooltipUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Camera worldSpaceCamera;
        [SerializeField] private RectTransform tooltipRoot;
        [SerializeField] private TextMeshProUGUI tooltipText;

        #endregion

        #region Callbacks

        public void ShowTooltip(TooltipArgs tooltipArgs)
        {
            tooltipText.text = tooltipArgs.text;
            tooltipRoot.transform.position = tooltipArgs.isWorldSpace ? 
                worldSpaceCamera.WorldToScreenPoint(new Vector3(tooltipArgs.position.x, tooltipArgs.position.y, -worldSpaceCamera.transform.position.z)) : tooltipArgs.position;
            tooltipRoot.gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            tooltipRoot.gameObject.SetActive(false);
        }

        #endregion
    }
}
