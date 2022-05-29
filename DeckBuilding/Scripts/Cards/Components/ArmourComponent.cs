using Celeste.Components;
using Celeste.DeckBuilding.Events;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Armour")]
    public class ArmourComponent : Components.Component
    {
        #region Save Data

        [Serializable]
        public class ArmourComponentData : ComponentData
        {
            public int Armour;
            public int MaxArmour;
        }

        #endregion

        #region Events

        public class ArmourComponentEvents : ComponentEvents
        {
            public ArmourChangedUnityEvent OnArmourChanged { get; } = new ArmourChangedUnityEvent();
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int initialArmour = 1;
        [SerializeField] private int maxArmour = 1;

        #endregion

        public override ComponentData CreateData()
        {
            ArmourComponentData armourData = new ArmourComponentData();
            armourData.Armour = initialArmour;
            armourData.MaxArmour = maxArmour;

            return armourData;
        }

        public override ComponentEvents CreateEvents()
        {
            return new ArmourComponentEvents();
        }

        public int GetMaxArmour(Instance instance)
        {
            ArmourComponentData armourData = instance.data as ArmourComponentData;
            return armourData.MaxArmour;
        }

        public void SetMaxArmour(Instance instance, int maxArmour)
        {
            ArmourComponentData armourData = instance.data as ArmourComponentData;
            if (armourData.MaxArmour != maxArmour)
            {
                armourData.MaxArmour = Mathf.Max(0, maxArmour);

                if (armourData.Armour > armourData.MaxArmour)
                {
                    // Clamp our armour so it's never bigger than our max armour
                    SetArmour(instance, armourData.MaxArmour);
                }
            }
        }

        public int GetArmour(Instance instance)
        {
            ArmourComponentData armourData = instance.data as ArmourComponentData;
            return armourData.Armour;
        }

        public void SetArmour(Instance instance, int armour)
        {
            ArmourComponentData armourData = instance.data as ArmourComponentData;
            if (armourData.Armour != armour)
            {
                int oldArmour = armourData.Armour;
                armourData.Armour = Mathf.Clamp(armour, 0, armourData.MaxArmour);

                ArmourComponentEvents events = instance.events as ArmourComponentEvents;
                events.OnArmourChanged.Invoke(new ArmourChangedArgs(oldArmour, armour));
            }
        }

        public void RemoveArmour(Instance instance, int armourToRemove)
        {
            ArmourComponentData armourData = instance.data as ArmourComponentData;
            if (armourToRemove != 0)
            {
                SetArmour(instance, armourData.Armour - armourToRemove);
            }
        }

        public void AddOnArmourChangedCallback(Instance instance, UnityAction<ArmourChangedArgs> callback)
        {
            ArmourComponentEvents events = instance.events as ArmourComponentEvents;
            events.OnArmourChanged.AddListener(callback);
        }

        public void RemoveOnArmourChangedCallback(Instance instance, UnityAction<ArmourChangedArgs> callback)
        {
            ArmourComponentEvents events = instance.events as ArmourComponentEvents;
            events.OnArmourChanged.RemoveListener(callback);
        }
    }
}