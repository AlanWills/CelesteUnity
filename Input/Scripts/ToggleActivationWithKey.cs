using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Toggle Activation With Key")]
    public class ToggleActivationWithKey : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObject toToggle;
        [SerializeField] private KeyCode toggleKey;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(toggleKey))
            {
                toToggle.SetActive(!toToggle.activeSelf);
            }
        }

        #endregion
    }
}
