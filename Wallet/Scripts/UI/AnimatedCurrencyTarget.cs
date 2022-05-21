using UnityEngine;

namespace Celeste.Wallet.UI
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Celeste/Wallet/UI/Animated Currency Target")]
    public class AnimatedCurrencyTarget : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Currency currency;
        [SerializeField] private AnimatedCurrencyTransformCache animatedCurrencyTransformCache;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            animatedCurrencyTransformCache.AddCurrencyTarget(currency, GetComponent<RectTransform>());
        }

        private void OnDisable()
        {
            animatedCurrencyTransformCache.RemoveCurrencyTarget(currency);
        }

        #endregion
    }
}
