using Celeste.Events;
using Celeste.Input;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Input
{
    [AddComponentMenu("Celeste/UI/Input/Tooltip UI")]
    public class TooltipUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private InputState inputState;
        [SerializeField] private Camera worldSpaceCamera;
        [SerializeField] private RectTransform tooltipRoot;
        [SerializeField] private TextMeshProUGUI tooltipText;

        private bool anchorToMouse = false;

        #endregion

        private void Update()
        {
            if (anchorToMouse)
            {
                tooltipRoot.position = inputState.MousePosition;
            }
        }

        #region Callbacks

        public void ShowTooltip(TooltipArgs tooltipArgs)
        {
            tooltipText.text = tooltipArgs.text;
            anchorToMouse = tooltipArgs.anchorToMouse;

            if (anchorToMouse)
            {
                tooltipRoot.position = tooltipArgs.isWorldSpace ?
                    worldSpaceCamera.WorldToScreenPoint(new Vector3(tooltipArgs.position.x, tooltipArgs.position.y, -worldSpaceCamera.transform.position.z)) : tooltipArgs.position;
            }
            else
            {
                tooltipRoot.position = inputState.MousePosition;
                anchorToMouse = true;
            }

            tooltipRoot.gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            anchorToMouse = false;
            tooltipRoot.gameObject.SetActive(false);
        }

        #endregion
    }
}
