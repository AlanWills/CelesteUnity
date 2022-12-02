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
    [DisplayName("Cost")]
    public class CostComponent : CardComponent
    {
        #region Save Data

        [Serializable]
        public class CostComponentData : ComponentData
        {
            public int Cost;
        }

        #endregion

        #region Events

        public class CostComponentEvents : ComponentEvents
        {
            public CostChangedUnityEvent OnCostChanged { get; } = new CostChangedUnityEvent();
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int initialCost;

        #endregion

        public override ComponentData CreateData()
        {
            CostComponentData costData = new CostComponentData();
            costData.Cost = initialCost;

            return costData;
        }

        public override ComponentEvents CreateEvents()
        {
            return new CostComponentEvents();
        }

        public int GetCost(Instance instance)
        {
            CostComponentData costData = instance.data as CostComponentData;
            return costData.Cost;
        }

        public void SetCost(Instance instance, CardRuntime card, int cost)
        {
            CostComponentData costData = instance.data as CostComponentData;
            if (costData.Cost != cost)
            {
                int oldCost = costData.Cost;
                costData.Cost = Mathf.Max(0, cost);

                CostComponentEvents events = instance.events as CostComponentEvents;
                events.OnCostChanged.Invoke(new CostChangedArgs(card, oldCost, cost));
            }
        }

        public void AddOnCostChangedCallback(Instance instance, UnityAction<CostChangedArgs> callback)
        {
            CostComponentEvents events = instance.events as CostComponentEvents;
            events.OnCostChanged.AddListener(callback);
        }

        public void RemoveOnCostChangedCallback(Instance instance, UnityAction<CostChangedArgs> callback)
        {
            CostComponentEvents events = instance.events as CostComponentEvents;
            events.OnCostChanged.RemoveListener(callback);
        }
    }
}