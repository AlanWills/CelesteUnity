using Celeste.DeckBuilding.Commands;
using Celeste.DeckBuilding.Events;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Damage Effect")]
    public class DamageEffectComponent : EffectComponent
    {
        #region Save Data

        [Serializable]
        public class DamageEffectComponentData : ComponentData
        {
            public int Damage;
        }

        #endregion

        #region Events

        public class DamageEffectComponentEvents : ComponentEvents
        {
            public DamageChangedUnityEvent OnDamageChanged { get; } = new DamageChangedUnityEvent();
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int initialDamage = 1;

        #endregion

        public override ComponentData CreateData()
        {
            DamageEffectComponentData damageData = new DamageEffectComponentData();
            damageData.Damage = initialDamage;

            return damageData;
        }

        public override ComponentEvents CreateEvents()
        {
            return new DamageEffectComponentEvents();
        }

        public override IDeckMatchCommand UseOn(Instance instance, CardRuntime target)
        {
            return new ApplyDamage(target, GetDamage(instance));
        }

        public int GetDamage(Instance instance)
        {
            DamageEffectComponentData saveData = instance.data as DamageEffectComponentData;
            return saveData.Damage;
        }

        public void SetDamage(Instance instance, int damage)
        {
            DamageEffectComponentData saveData = instance.data as DamageEffectComponentData;
            if (saveData.Damage != damage)
            {
                int oldDamage = saveData.Damage;
                saveData.Damage = damage;

                DamageEffectComponentEvents events = instance.events as DamageEffectComponentEvents;
                events.OnDamageChanged.Invoke(new DamageChangedArgs(oldDamage, damage));
            }
        }

        public void AddOnDamageChangedCallback(Instance instance, UnityAction<DamageChangedArgs> callback)
        {
            DamageEffectComponentEvents events = instance.events as DamageEffectComponentEvents;
            events.OnDamageChanged.AddListener(callback);
        }

        public void RemoveOnDamageChangedCallback(Instance instance, UnityAction<DamageChangedArgs> callback)
        {
            DamageEffectComponentEvents events = instance.events as DamageEffectComponentEvents;
            events.OnDamageChanged.RemoveListener(callback);
        }
    }
}