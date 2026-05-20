using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Extensions
{
    public static class HealthComponentExtensions
    {
        public static bool SupportsHealth(this CardInstance card)
        {
            return card.HasComponent<HealthComponent>();
        }

        public static int GetMaxHealth(this CardInstance card)
        {
            var healthComponent = card.FindComponent<HealthComponent>();
#if COMPONENT_CHECKS
            if (!healthComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(HealthComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return healthComponent.component.GetMaxHealth(healthComponent.instance);
        }

        public static void SetMaxHealth(this CardInstance card, int health)
        {
            var healthComponent = card.FindComponent<HealthComponent>();
#if COMPONENT_CHECKS
            if (!healthComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to set MaxHealth on a Card with no {nameof(HealthComponent)}.");
                return;
            }
#endif
            healthComponent.component.SetMaxHealth(healthComponent.instance, card, health);
        }

        public static void IncreaseMaxHealth(this CardInstance card, int increaseAmount)
        {
            UnityEngine.Debug.Assert(increaseAmount >= 0, $"{increaseAmount} is not a valid value.");
            SetMaxHealth(card, GetMaxHealth(card) + increaseAmount);
        }

        public static void DecreaseMaxHealth(this CardInstance card, int decreaseAmount)
        {
            UnityEngine.Debug.Assert(decreaseAmount >= 0, $"{decreaseAmount} is not a valid value.");
            SetMaxHealth(card, GetMaxHealth(card) - decreaseAmount);
        }

        public static int GetHealth(this CardInstance card)
        {
            var healthComponent = card.FindComponent<HealthComponent>();
#if COMPONENT_CHECKS
            if (!healthComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(HealthComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return healthComponent.component.GetHealth(healthComponent.instance);
        }

        public static void SetHealth(this CardInstance card, int health)
        {
            var healthComponent = card.FindComponent<HealthComponent>();
#if COMPONENT_CHECKS
            if (!healthComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to set Health on a Card with no {nameof(HealthComponent)}.");
                return;
            }
#endif
            healthComponent.component.SetHealth(healthComponent.instance, card, health);
        }

        public static void RemoveHealth(this CardInstance card, int healthToRemove)
        {
            SetHealth(card, GetHealth(card) - healthToRemove);
        }

        public static void AddHealth(this CardInstance card, int healthToAdd)
        {
            SetHealth(card, GetHealth(card) + healthToAdd);
        }

        public static void AddOnHealthChangedCallback(this CardInstance card, UnityAction<HealthChangedArgs> callback)
        {
            var healthComponent = card.FindComponent<HealthComponent>();
#if COMPONENT_CHECKS
            if (!healthComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to add OnHealthChanged callback on a Card with no {nameof(HealthComponent)}.");
                return;
            }
#endif
            healthComponent.component.AddOnHealthChangedCallback(healthComponent.instance, callback);
        }

        public static void RemoveOnHealthChangedCallback(this CardInstance card, UnityAction<HealthChangedArgs> callback)
        {
            var healthComponent = card.FindComponent<HealthComponent>();
#if COMPONENT_CHECKS
            if (!healthComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to remove OnHealthChanged callback on a Card with no {nameof(HealthComponent)}.");
                return;
            }
#endif
            healthComponent.component.RemoveOnHealthChangedCallback(healthComponent.instance, callback);
        }

        public static void AddOnDieCallback(this CardInstance card, UnityAction<DieArgs> callback)
        {
            var healthComponent = card.FindComponent<HealthComponent>();
#if COMPONENT_CHECKS
            if (!healthComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to add OnDie callback on a Card with no {nameof(HealthComponent)}.");
                return;
            }
#endif
            healthComponent.component.AddOnDieCallback(healthComponent.instance, callback);
        }

        public static void RemoveOnDieCallback(this CardInstance card, UnityAction<DieArgs> callback)
        {
            var healthComponent = card.FindComponent<HealthComponent>();
#if COMPONENT_CHECKS
            if (!healthComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to remove OnDie callback on a Card with no {nameof(HealthComponent)}.");
                return;
            }
#endif
            healthComponent.component.RemoveOnDieCallback(healthComponent.instance, callback);
        }
    }
}