using Celeste.DeckBuilding.Events;
using Celeste.Events;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Combat")]
    public class CombatComponent : Component
    {
        #region Save Data

        [Serializable]
        public class CombatComponentData : ComponentData
        {
            public int Strength;
            public bool Ready;
        }

        #endregion

        #region Events

        public class CombatComponentEvents : ComponentEvents
        {
            public BoolUnityEvent OnReadyChanged { get; } = new BoolUnityEvent();
            public StrengthChangedUnityEvent OnStrengthChanged { get; } = new StrengthChangedUnityEvent();
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int baseStrength = 1;

        #endregion

        public override ComponentData CreateData()
        {
            CombatComponentData damageData = new CombatComponentData();
            damageData.Strength = baseStrength;

            return damageData;
        }

        public override ComponentEvents CreateEvents()
        {
            return new CombatComponentEvents();
        }

        public int GetStrength(Instance instance)
        {
            CombatComponentData saveData = instance.data as CombatComponentData;
            return saveData.Strength;
        }

        public void SetStrength(Instance instance, int strength)
        {
            CombatComponentData saveData = instance.data as CombatComponentData;
            if (saveData.Strength != strength)
            {
                int oldStrength = saveData.Strength;
                saveData.Strength = strength;

                CombatComponentEvents combatEvents = (instance.events as CombatComponentEvents);
                combatEvents.OnStrengthChanged.Invoke(new StrengthChangedArgs(oldStrength, strength));
            }
        }

        public bool IsReady(Instance instance)
        {
            CombatComponentData saveData = instance.data as CombatComponentData;
            return saveData.Ready;
        }

        public void SetReady(Instance instance, bool isReady)
        {
            CombatComponentData saveData = instance.data as CombatComponentData;
            if (saveData.Ready != isReady)
            {
                saveData.Ready = isReady;
                
                CombatComponentEvents events = instance.events as CombatComponentEvents;
                events.OnReadyChanged.Invoke(isReady);
            }
        }

        public void AddOnReadyChangedCallback(Instance instance, UnityAction<bool> callback)
        {
            CombatComponentEvents events = instance.events as CombatComponentEvents;
            events.OnReadyChanged.AddListener(callback);
        }

        public void RemoveOnReadyChangedCallback(Instance instance, UnityAction<bool> callback)
        {
            CombatComponentEvents events = instance.events as CombatComponentEvents;
            events.OnReadyChanged.RemoveListener(callback);
        }

        public void AddOnStrengthChangedCallback(Instance instance, UnityAction<StrengthChangedArgs> callback)
        {
            CombatComponentEvents events = instance.events as CombatComponentEvents;
            events.OnStrengthChanged.AddListener(callback);
        }

        public void RemoveOnStrengthChangedCallback(Instance instance, UnityAction<StrengthChangedArgs> callback)
        {
            CombatComponentEvents events = instance.events as CombatComponentEvents;
            events.OnStrengthChanged.RemoveListener(callback);
        }
    }
}