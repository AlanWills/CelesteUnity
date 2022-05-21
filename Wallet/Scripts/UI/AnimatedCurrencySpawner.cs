using Celeste.Memory;
using Celeste.Wallet.Events;
using System.Collections;
using UnityEngine;

namespace Celeste.Wallet.UI
{
    [AddComponentMenu("Celeste/Wallet/UI/Animated Currency Spawner")]
    public class AnimatedCurrencySpawner : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private AnimatedCurrencyTransformCache animatedCurrencyTransformCache;
        [SerializeField] private GameObjectAllocator animatedCurrencyAllocator;
        [SerializeField] private RectTransform fallbackSource;
        [SerializeField] private float animationDuration = 1;
        [SerializeField] private float spawnDelay = 0.05f;

        #endregion

        public void SpawnForChangeOfCurrency(Currency currency, int quantityChange)
        {
            // If we add currency we animate from source (which we try to find) to animated currency target.  If we have no source cached, we use the fallbackSource.
            // If we remove currency we reverse this process internally, but still try to find a registered custom source.

            if (quantityChange == 0)
            {
                return;
            }

            if (!animatedCurrencyTransformCache.TryFindCurrencySource(currency, out RectTransform source))
            {
                UnityEngine.Debug.Log($"{currency.name}AnimatedCurrencySource not cached, so using fallback source.");
                source = fallbackSource;
            }

            if (animatedCurrencyTransformCache.TryFindCurrencyTarget(currency, out RectTransform target))
            {
                StartCoroutine(Spawn(currency, quantityChange, source, target));
            }
            else
            {
                UnityEngine.Debug.LogAssertion($"Could not find animated currency target for currency {currency}.");
            }
        }

        private IEnumerator Spawn(
            Currency currency, 
            int quantityChange,
            RectTransform source,
            RectTransform target)
        {
            if (quantityChange < 0)
            {
                // Swap the source and target so the animation goes in the other direction
                RectTransform temp = source;
                source = target;
                target = temp;
            }

            // Use the min of our change and how many we can actually allocate
            int numToSpawn = Mathf.Min(Mathf.Abs(quantityChange), animatedCurrencyAllocator.Available);

            for (int i = 0; i < numToSpawn; i++)
            {
                GameObject gameObject = animatedCurrencyAllocator.Allocate();
                AnimatedCurrencyController animatedCurrencyController = gameObject.GetComponent<AnimatedCurrencyController>();
                animatedCurrencyController.Hookup(
                    currency,
                    source,
                    target,
                    animationDuration,
                    (AnimatedCurrencyController controller) => animatedCurrencyAllocator.Deallocate(controller.gameObject));
                gameObject.SetActive(true);

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        #region Callbacks

        public void OnCurrencyChanged(CurrencyChangedArgs currencyChangedArgs)
        {
            SpawnForChangeOfCurrency(currencyChangedArgs.currency, currencyChangedArgs.quantity);
        }

        #endregion
    }
}
