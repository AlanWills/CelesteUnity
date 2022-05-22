using Celeste.Parameters;
using Celeste.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Features.UI
{
    [AddComponentMenu("Celeste/Features/UI/Toggle Interactable For Feature")]
    public class ToggleInteractableForFeature : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Feature feature;
        [SerializeField] private Selectable selectableToToggle;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref selectableToToggle);
        }

        private void Awake()
        {
            SetInteractable(feature.IsEnabled);

            feature.AddOnEnabledChangedCallback(OnEnabledChanged);
        }

        private void OnDestroy()
        {
            feature.RemoveOnEnabledChangedCallback(OnEnabledChanged);
        }

        #endregion

        private void SetInteractable(bool interactable)
        {
            selectableToToggle.interactable = interactable;
        }

        #region Callbacks

        private void OnEnabledChanged(ValueChangedArgs<bool> args)
        {
            SetInteractable(args.newValue);
        }

        #endregion
    }
}
