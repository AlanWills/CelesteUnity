using Celeste.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Celeste.UI
{
    [AddComponentMenu("Celeste/UI/Detail UI Manager")]
    public class DetailUIManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private List<DetailUIController> customDetailUIControllers = new List<DetailUIController>();
        [SerializeField] private InputAction showMoreDetailInputAction;
        [SerializeField] private InputState inputState;

        private GameObject currentDetailGameObject;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            showMoreDetailInputAction.Enable();
            showMoreDetailInputAction.started += OnShowMoreDetailInputPressed;
            showMoreDetailInputAction.canceled += OnShowMoreDetailInputReleased;
        }

        #endregion

        private void ShowDetail(GameObject currentDetailGameObject, IDetail detail)
        {
            this.currentDetailGameObject = currentDetailGameObject;

            IDetailContext detailContext = detail.CreateDetailContext();

            for (int i = 0, n = customDetailUIControllers.Count; i < n; i++)
            {
                DetailUIController detailUIController = customDetailUIControllers[i];

                if (detailUIController.IsValidFor(detailContext))
                {
                    detailUIController.Show(detailContext);
                }
                else
                {
                    detailUIController.Hide();
                }
            }
        }

        private void HideDetail()
        {
            for (int i = 0, n = customDetailUIControllers.Count; i < n; i++)
            {
                customDetailUIControllers[i].Hide();
            }

            currentDetailGameObject = null;
        }

        #region Callbacks

        public void OnPointerEnterAnyGameObject(GameObject gameObject)
        {
            if (gameObject != currentDetailGameObject)
            {
                HideDetail();

                if (gameObject != null && 
                    showMoreDetailInputAction.IsPressed() &&
                    gameObject.TryGetComponent<IDetail>(out var detail))
                {
                    ShowDetail(gameObject, detail);
                }
            }
        }

        public void OnPointerExitAnyGameObject(GameObject gameObject)
        {
            HideDetail();
        }

        private void OnShowMoreDetailInputPressed(CallbackContext context)
        {
            OnPointerEnterAnyGameObject(inputState.HitGameObject);
        }

        private void OnShowMoreDetailInputReleased(CallbackContext context)
        {
            OnPointerExitAnyGameObject(inputState.HitGameObject);
        }

        #endregion
    }
}
