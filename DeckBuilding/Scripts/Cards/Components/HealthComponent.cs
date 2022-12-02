using Celeste.Components;
using Celeste.DeckBuilding.Components;
using Celeste.DeckBuilding.Events;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Health")]
    public class HealthComponent : CardComponent
    {
        #region Save Data

        [Serializable]
        public class HealthComponentData : ComponentData
        {
            public int Health;
            public int MaxHealth;
        }

        #endregion

        #region Events

        public class HealthComponentEvents : ComponentEvents
        {
            public HealthChangedUnityEvent OnHealthChanged { get; } = new HealthChangedUnityEvent();
            public DieUnityEvent OnDie { get; } = new DieUnityEvent();
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int initialHealth = 1;
        [SerializeField] private int maxHealth = 1;

        #endregion

        public override ComponentData CreateData()
        {
            HealthComponentData healthData = new HealthComponentData();
            healthData.Health = initialHealth;
            healthData.MaxHealth = maxHealth;

            return healthData;
        }

        public override ComponentEvents CreateEvents()
        {
            return new HealthComponentEvents();
        }

        public int GetMaxHealth(Instance instance)
        {
            HealthComponentData healthData = instance.data as HealthComponentData;
            return healthData.MaxHealth;
        }

        public void SetMaxHealth(Instance instance, CardRuntime card, int maxHealth)
        {
            HealthComponentData healthData = instance.data as HealthComponentData;
            if (healthData.MaxHealth != maxHealth)
            {
                healthData.MaxHealth = Mathf.Max(0, maxHealth);

                if (healthData.Health > healthData.MaxHealth)
                {
                    // Clamp our health so it's never bigger than our max armour
                    SetHealth(instance, card, healthData.MaxHealth);
                }
            }
        }

        public int GetHealth(Instance instance)
        {
            HealthComponentData healthData = instance.data as HealthComponentData;
            return healthData.Health;
        }

        public void SetHealth(Instance instance, CardRuntime card, int newHealth)
        {
            HealthComponentData healthData = instance.data as HealthComponentData;
            if (healthData.Health != newHealth)
            {
                int oldHealth = healthData.Health;
                healthData.Health = Mathf.Clamp(newHealth, 0, healthData.MaxHealth);

                HealthComponentEvents events = instance.events as HealthComponentEvents;
                events.OnHealthChanged.Invoke(new HealthChangedArgs(oldHealth, newHealth));

                if (healthData.Health == 0)
                {
                    events.OnDie.Invoke(new DieArgs(card));
                }
            }
        }

        public void RemoveHealth(Instance instance, CardRuntime card, int amount)
        {
            HealthComponentData healthData = instance.data as HealthComponentData;
            if (amount != 0)
            {
                SetHealth(instance, card, healthData.Health - amount);
            }
        }

        public void AddOnHealthChangedCallback(Instance instance, UnityAction<HealthChangedArgs> callback)
        {
            HealthComponentEvents events = instance.events as HealthComponentEvents;
            events.OnHealthChanged.AddListener(callback);
        }

        public void RemoveOnHealthChangedCallback(Instance instance, UnityAction<HealthChangedArgs> callback)
        {
            HealthComponentEvents events = instance.events as HealthComponentEvents;
            events.OnHealthChanged.RemoveListener(callback);
        }

        public void AddOnDieCallback(Instance instance, UnityAction<DieArgs> callback)
        {
            HealthComponentEvents events = instance.events as HealthComponentEvents;
            events.OnDie.AddListener(callback);
        }

        public void RemoveOnDieCallback(Instance instance, UnityAction<DieArgs> callback)
        {
            HealthComponentEvents events = instance.events as HealthComponentEvents;
            events.OnDie.RemoveListener(callback);
        }
    }
}