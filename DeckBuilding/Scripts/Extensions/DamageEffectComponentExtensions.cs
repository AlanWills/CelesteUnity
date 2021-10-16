using Celeste.DeckBuilding.Cards;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class DamageEffectComponentExtensions
    {
        public static bool SupportsDamageEffect(this CardRuntime card)
        {
            return card.HasComponent<DamageEffectComponent>();
        }

        public static int GetDamage(this CardRuntime card)
        {
            var damageComponent = card.FindComponent<DamageEffectComponent>();
#if COMPONENT_CHECKS
            if (!damageComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(DamageEffectComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return damageComponent.component.GetDamage(damageComponent.instance);
        }

        public static void SetDamage(this CardRuntime card, int damage)
        {
            var damageComponent = card.FindComponent<DamageEffectComponent>();
#if COMPONENT_CHECKS
            if (!damageComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to set Damage on a Card with no {nameof(DamageEffectComponent)}.");
                return;
            }
#endif
            damageComponent.component.SetDamage(damageComponent.instance, Mathf.Max(0, damage));
        }

        public static void ApplyDamage(this CardRuntime card, int damage)
        {
            if (card.SupportsArmour())
            {
                int armourToRemove = Mathf.Min(card.GetArmour(), damage);
                card.RemoveArmour(armourToRemove);
                damage -= armourToRemove;
            }

            if (card.SupportsHealth())
            {
                // Important to still do this, even if the damage is zero
                // We may have visual effects or other effects that apply when damage is taken
                // Even if that damage is zero (e.g. retaliation)
                card.RemoveHealth(Mathf.Min(card.GetHealth(), damage));
            }
        }
    }
}