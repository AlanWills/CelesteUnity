using Celeste.Tools.Attributes.GUI;
using System.Collections;
using UnityEngine;

namespace Celeste.FX
{
    [AddComponentMenu("Celeste/FX/Disable After")]
    public class DisableAfter : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField, Tools.Attributes.GUI.Min(0), ReadOnlyAtRuntime] private float seconds = 1;

        private WaitForSeconds waitForSeconds;
        private Coroutine disableCoroutine;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            waitForSeconds = new WaitForSeconds(seconds);
        }

        private void OnEnable()
        {
            if (disableCoroutine != null)
            {
                StopCoroutine(disableCoroutine);
            }

            disableCoroutine = StartCoroutine(Wait());
        }

        #endregion

        private IEnumerator Wait()
        {
            yield return waitForSeconds;

            gameObject.SetActive(false);
        }
    }
}
