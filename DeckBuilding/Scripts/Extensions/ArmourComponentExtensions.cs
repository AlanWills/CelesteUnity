using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Extensions
{
    public static class ArmourComponentExtensions
    {
        public static bool SupportsArmour(this CardRuntime card)
        {
            return card.HasComponent<ArmourComponent>();
        }

        public static int GetMaxArmour(this CardRuntime card)
        {
            var armourComponent = card.FindComponent<ArmourComponent>();
#if COMPONENT_CHECKS
            if (!armourComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(ArmourComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return armourComponent.component.GetMaxArmour(armourComponent.instance);
        }

        public static void SetMaxArmour(this CardRuntime card, int maxArmour)
        {
            var armourComponent = card.FindComponent<ArmourComponent>();
#if COMPONENT_CHECKS
            if (!armourComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to set MaxArmour on a Card with no {nameof(ArmourComponent)}.");
                return;
            }
#endif
            armourComponent.component.SetMaxArmour(armourComponent.instance, maxArmour);
        }

        public static void IncreaseMaxArmour(this CardRuntime card, int increaseAmount)
        {
            UnityEngine.Debug.Assert(increaseAmount >= 0, $"{increaseAmount} is not a valid value.");
            SetMaxArmour(card, GetMaxArmour(card) + increaseAmount);
        }

        public static void DecreaseMaxArmour(this CardRuntime card, int decreaseAmount)
        {
            UnityEngine.Debug.Assert(decreaseAmount >= 0, $"{decreaseAmount} is not a valid value.");
            SetMaxArmour(card, GetMaxArmour(card) - decreaseAmount);
        }

        public static int GetArmour(this CardRuntime card)
        {
            var armourComponent = card.FindComponent<ArmourComponent>();
#if COMPONENT_CHECKS
            if (!armourComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(ArmourComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return armourComponent.component.GetArmour(armourComponent.instance);
        }

        public static void SetArmour(this CardRuntime card, int armour)
        {
            var armourComponent = card.FindComponent<ArmourComponent>();
#if COMPONENT_CHECKS
            if (!armourComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to set Armour on a Card with no {nameof(ArmourComponent)}.");
                return;
            }
#endif
            armourComponent.component.SetArmour(armourComponent.instance, armour);
        }

        public static void AddArmour(this CardRuntime card, int armourToAdd)
        {
            SetArmour(card, GetArmour(card) + armourToAdd);
        }

        public static void RemoveArmour(this CardRuntime card, int armourToRemove)
        {
            SetArmour(card, GetArmour(card) - armourToRemove);
        }

        public static void AddOnArmourChangedCallback(this CardRuntime card, UnityAction<ArmourChangedArgs> callback)
        {
            var armourComponent = card.FindComponent<ArmourComponent>();
#if COMPONENT_CHECKS
            if (!armourComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to add OnArmourChanged callback on a Card with no {nameof(ArmourComponent)}.");
                return;
            }
#endif
            armourComponent.component.AddOnArmourChangedCallback(armourComponent.instance, callback);
        }

        public static void RemoveOnArmourChangedCallback(this CardRuntime card, UnityAction<ArmourChangedArgs> callback)
        {
            var armourComponent = card.FindComponent<ArmourComponent>();
#if COMPONENT_CHECKS
            if (!armourComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to remove OnArmourChanged callback on a Card with no {nameof(ArmourComponent)}.");
                return;
            }
#endif
            armourComponent.component.RemoveOnArmourChangedCallback(armourComponent.instance, callback);
        }
    }
}