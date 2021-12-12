using UnityEngine;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Toggle Activation With Key")]
    public class ToggleActivationWithKey : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObject toToggle;

        #endregion

        public void Toggle()
        {
            toToggle.SetActive(!toToggle.activeSelf);
        }
    }
}
