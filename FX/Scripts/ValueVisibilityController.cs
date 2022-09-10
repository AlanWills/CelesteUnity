using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.FX
{
    [AddComponentMenu("Celeste/FX/Value Visibility Controller")]
    public class ValueVisibilityController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObject target;
        [SerializeField] private BoolValue isVisible;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            if (target == gameObject)
            {
                Debug.LogAssertion($"Should not assign the same gameobject the {nameof(ValueVisibilityController)} is added to to the 'target' field on the script.");
                target = null;
            }
        }

        private void OnEnable()
        {
            target.SetActive(isVisible.Value);
            isVisible.AddValueChangedCallback(OnIsVisibleChanged);
        }

        private void OnDisable()
        {
            isVisible.RemoveValueChangedCallback(OnIsVisibleChanged);
        }

        #endregion

        #region Callbacks

        private void OnIsVisibleChanged(ValueChangedArgs<bool> valueChangedArgs)
        {
            target.SetActive(valueChangedArgs.newValue);
        }

        #endregion
    }
}
