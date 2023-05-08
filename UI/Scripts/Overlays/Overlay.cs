using Celeste.Events;
using Celeste.UI.Events;
using Celeste.UI.Popups;
using System.Collections;
using UnityEngine;

namespace Celeste.UI.Overlays
{
    [AddComponentMenu("Celeste/UI/Overlay")]
    public class Overlay : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObject overlayRoot;

        [Header("Required Events")]
        [SerializeField] private ShowOverlayEvent showOverlay;

        [Header("Optional Events")]
        [SerializeField] private Celeste.Events.Event onHideOverlay;

        private IOverlayController overlayController;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Debug.Assert(showOverlay != null, $"No '{nameof(ShowOverlayEvent)}' found on {nameof(Overlay)} {gameObject.name}.");
            showOverlay.AddListener(OnShowOverlay);

            overlayController = GetComponent<IOverlayController>();
        }

        private void OnDestroy()
        {
            showOverlay.RemoveListener(OnShowOverlay);
        }

        #endregion

        #region Show/Hide

        private void Show(IOverlayArgs args)
        {
            overlayRoot.SetActive(true);

            if (overlayController != null)
            {
                overlayController.OnShow(args);
            }
        }

        public void Hide()
        {
            if (overlayController != null)
            {
                overlayController.OnHide();
            }

            if (onHideOverlay != null)
            {
                onHideOverlay.Invoke();
            }

            overlayRoot.SetActive(false);
        }

        #endregion

        #region Callbacks

        private void OnShowOverlay(IOverlayArgs args)
        {
            Show(args);
        }

        #endregion
    }
}
