using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Extensions
{
    public static class HealthComponentExtensions
    {
        public static bool SupportsHealth(this CardRuntime card)
        {
            return card.HasComponent<HealthComponent>();
        }

        public static int GetMaxHealth(this CardRuntime card)
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

        public static void SetMaxHealth(this CardRuntime card, int health)
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

        public static void IncreaseMaxHealth(this CardRuntime card, int increaseAmount)
        {
            UnityEngine.Debug.Assert(increaseAmount >= 0, $"{increaseAmount} is not a valid value.");
            SetMaxHealth(card, GetMaxHealth(card) + increaseAmount);
        }

        public static void DecreaseMaxHealth(this CardRuntime card, int decreaseAmount)
        {
            UnityEngine.Debug.Assert(decreaseAmount >= 0, $"{decreaseAmount} is not a valid value.");
            SetMaxHealth(card, GetMaxHealth(card) - decreaseAmount);
        }

        public static int GetHealth(this CardRuntime card)
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

        public static void SetHealth(this CardRuntime card, int health)
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

        public static void RemoveHealth(this CardRuntime card, int healthToRemove)
        {
            SetHealth(card, GetHealth(card) - healthToRemove);
        }

        public static void AddHealth(this CardRuntime card, int healthToAdd)
        {
            SetHealth(card, GetHealth(card) + healthToAdd);
        }

        public static void AddOnHealthChangedCallback(this CardRuntime card, UnityAction<HealthChangedArgs> callback)
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

        public static void RemoveOnHealthChangedCallback(this CardRuntime card, UnityAction<HealthChangedArgs> callback)
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

        public static void AddOnDieCallback(this CardRuntime card, UnityAction<DieArgs> callback)
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

        public static void RemoveOnDieCallback(this CardRuntime card, UnityAction<DieArgs> callback)
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