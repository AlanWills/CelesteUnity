using Celeste.Events;
using UnityEngine;

namespace Celeste.Features.UI
{
    [AddComponentMenu("Celeste/Features/UI/Toggle Active For Feature")]
    public class ToggleActiveForFeature : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Feature feature;
        [SerializeField] private GameObject gameObjectToToggle;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (gameObjectToToggle == null)
            {
                gameObjectToToggle = gameObject;
            }
        }

        private void Awake()
        {
            SetActive(feature.IsEnabled);

            feature.AddOnEnabledChangedCallback(OnEnabledChanged);
        }

        private void OnDestroy()
        {
            feature.RemoveOnEnabledChangedCallback(OnEnabledChanged);
        }

        #endregion

        private void SetActive(bool isActive)
        {
            gameObjectToToggle.SetActive(isActive);
        }

        #region Callbacks

        private void OnEnabledChanged(ValueChangedArgs<bool> args)
        {
            SetActive(args.newValue);
        }

        #endregion
    }
}
