using Celeste.Components;
using Celeste.DeckBuilding.Commands;
using Celeste.DeckBuilding.Logic;
using UnityEngine;

namespace Celeste.DeckBuilding.Cards
{
    public abstract class EffectComponent : Components.Component, IRequiresTarget
    {
        #region Properties and Fields

        [SerializeField] private bool singleTarget = true;
        [SerializeField] private EffectRequirement[] effectRequirements;

        #endregion

        public bool RequiresTarget(Instance instance)
        {
            return singleTarget;
        }

        public bool CanUseOn(Instance instance, CardRuntime effect, CardRuntime target)
        {
            EffectRequirementArgs args = new EffectRequirementArgs()
            {
                effect = effect,
                target = target
            };

            for (int i = 0, n = effectRequirements != null ? effectRequirements.Length : 0; i < n; ++i)
            {
                if (!effectRequirements[i].Check(args))
                {
                    return false;
                }
            }

            return true;
        }

        public abstract IDeckMatchCommand UseOn(Instance instance, CardRuntime target);
    }
}