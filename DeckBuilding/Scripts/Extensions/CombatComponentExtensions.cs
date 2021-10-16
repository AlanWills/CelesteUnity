using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static Celeste.DeckBuilding.Cards.CombatComponent;

namespace Celeste.DeckBuilding.Extensions
{
    public static class CombatComponentExtensions
    {
        public static bool SupportsCombat(this CardRuntime card)
        {
            return card.HasComponent<CombatComponent>();
        }

        public static bool IsReady(this CardRuntime card)
        {
            var combatComponent = card.FindComponent<CombatComponent>();
#if COMPONENT_CHECKS
            if (!combatComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(CombatComponent)} on card {card.CardName}.");
                return false;
            }
#endif
            return combatComponent.component.IsReady(combatComponent.instance);
        }

        public static void SetReady(this CardRuntime card, bool ready)
        {
            var combatComponent = card.FindComponent<CombatComponent>();
#if COMPONENT_CHECKS
            if (!combatComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to set Ready on a Card with no {nameof(CombatComponent)}.");
                return;
            }
#endif
            combatComponent.component.SetReady(combatComponent.instance, ready);
        }


        public static void Refresh(this CardRuntime card)
        {
            if (card.SupportsCombat())
            {
                card.SetReady(true);
            }
        }

        public static void Exhaust(this CardRuntime card)
        {
            if (card.SupportsCombat())
            {
                card.SetReady(false);
            }
        }
        public static int GetStrength(this CardRuntime card)
        {
            var combatComponent = card.FindComponent<CombatComponent>();
#if COMPONENT_CHECKS
            if (!combatComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(CombatComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return combatComponent.component.GetStrength(combatComponent.instance);
        }

        public static void SetStrength(this CardRuntime card, int strength)
        {
            var combatComponent = card.FindComponent<CombatComponent>();
#if COMPONENT_CHECKS
            if (!combatComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to set Strength on a Card with no {nameof(CombatComponent)}.");
                return;
            }
#endif
            combatComponent.component.SetStrength(combatComponent.instance, Mathf.Max(0, strength));
        }

        public static void AddOnStrengthChangedCallback(this CardRuntime card, UnityAction<StrengthChangedArgs> callback)
        {
            var combatComponent = card.FindComponent<CombatComponent>();
#if COMPONENT_CHECKS
            if (!combatComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to add OnStrengthChanged callback on a Card with no {nameof(CombatComponent)}.");
                return;
            }
#endif
            CombatComponentEvents events = combatComponent.instance.events as CombatComponentEvents;
            events.OnStrengthChanged.AddListener(callback);
        }

        public static void RemoveOnStrengthChangedCallback(this CardRuntime card, UnityAction<StrengthChangedArgs> callback)
        {
            var combatComponent = card.FindComponent<CombatComponent>();
#if COMPONENT_CHECKS
            if (!combatComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to remove OnStrengthChanged callback on a Card with no {nameof(CombatComponent)}.");
                return;
            }
#endif
            CombatComponentEvents events = combatComponent.instance.events as CombatComponentEvents;
            events.OnStrengthChanged.RemoveListener(callback);
        }

        public static void AddOnReadyChangedCallback(this CardRuntime card, UnityAction<bool> callback)
        {
            var combatComponent = card.FindComponent<CombatComponent>();
#if COMPONENT_CHECKS
            if (!combatComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to add OnReadyChanged callback on a Card with no {nameof(CombatComponent)}.");
                return;
            }
#endif
            CombatComponentEvents events = combatComponent.instance.events as CombatComponentEvents;
            events.OnReadyChanged.AddListener(callback);
        }

        public static void RemoveOnReadyChangedCallback(this CardRuntime card, UnityAction<bool> callback)
        {
            var combatComponent = card.FindComponent<CombatComponent>();
#if COMPONENT_CHECKS
            if (!combatComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to remove OnReadyChanged callback on a Card with no {nameof(CombatComponent)}.");
                return;
            }
#endif
            CombatComponentEvents events = combatComponent.instance.events as CombatComponentEvents;
            events.OnReadyChanged.RemoveListener(callback);
        }
    }
}