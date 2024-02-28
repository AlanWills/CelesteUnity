using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Wallet.UI
{
    [CreateAssetMenu(fileName = nameof(AnimatedCurrencyTransformCache), order = CelesteMenuItemConstants.WALLET_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.WALLET_MENU_ITEM + "UI/Animated Currency Transform Cache")]
    public class AnimatedCurrencyTransformCache : ScriptableObject
    {
        #region Properties and Fields

        [NonSerialized] private Dictionary<Currency, RectTransform> currencySourceCache = new Dictionary<Currency, RectTransform>();
        [NonSerialized] private Dictionary<Currency, RectTransform> currencyTargetCache = new Dictionary<Currency, RectTransform>();

        #endregion

        public void AddCurrencySource(Currency currency, RectTransform target)
        {
            if (currencySourceCache.ContainsKey(currency))
            {
                currencySourceCache[currency] = target;
            }
            else
            {
                currencySourceCache.Add(currency, target);
            }
        }

        public void AddCurrencyTarget(Currency currency, RectTransform target)
        {
            if (currencyTargetCache.ContainsKey(currency))
            {
                currencyTargetCache[currency] = target;
            }
            else
            {
                currencyTargetCache.Add(currency, target);
            }
        }

        public void RemoveCurrencySource(Currency currency)
        {
            currencySourceCache.Remove(currency);
        }

        public void RemoveCurrencyTarget(Currency currency)
        {
            currencyTargetCache.Remove(currency);
        }

        public bool TryFindCurrencySource(Currency currency, out RectTransform transform)
        {
            return currencySourceCache.TryGetValue(currency, out transform);
        }

        public bool TryFindCurrencyTarget(Currency currency, out RectTransform transform)
        {
            return currencyTargetCache.TryGetValue(currency, out transform);
        }
    }
}
