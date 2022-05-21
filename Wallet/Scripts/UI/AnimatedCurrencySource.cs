using UnityEngine;

namespace Celeste.Wallet.UI
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Celeste/Wallet/UI/Animated Currency Source")]
    public class AnimatedCurrencySource : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Currency currency;
        [SerializeField] private AnimatedCurrencyTransformCache animatedCurrencyTransformCache;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            animatedCurrencyTransformCache.AddCurrencySource(currency, GetComponent<RectTransform>());
        }

        private void OnDisable()
        {
            animatedCurrencyTransformCache.RemoveCurrencySource(currency);
        }

        #endregion
    }
}
