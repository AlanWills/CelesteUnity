using Celeste.Events;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Input
{
    [AddComponentMenu("Celeste/UI/Input/Tooltip UI")]
    public class TooltipUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Canvas tooltipCanvas;
        [SerializeField] private Camera worldSpaceCamera;
        [SerializeField] private RectTransform tooltipRoot;
        [SerializeField] private TextMeshProUGUI tooltipText;

        #endregion

        #region Callbacks

        public void ShowTooltip(TooltipArgs tooltipArgs)
        {
            //Vector2 pos = tooltipArgs.position;  // get the game object position
            //Vector2 viewportPoint = worldSpaceCamera.WorldToViewportPoint(pos);  //convert game object position to VievportPoint

            //// set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
            //tooltipRoot.anchorMin = viewportPoint;
            //tooltipRoot.anchorMax = viewportPoint;

            tooltipText.text = tooltipArgs.text;
            //tooltipRoot.position = tooltipArgs.position;
            //Vector3 position = worldSpaceCamera.WorldToScreenPoint(new Vector3(tooltipArgs.position.x, tooltipArgs.position.y, -worldSpaceCamera.transform.position.z));
            //tooltipRoot.transform.position = tooltipCanvas.GetComponent<RectTransform>().InverseTransformVector(tooltipArgs.position);
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
