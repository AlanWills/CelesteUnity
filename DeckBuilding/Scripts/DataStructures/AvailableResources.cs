﻿using Celeste.Events;
using UnityEngine;

namespace Celeste.DeckBuilding
{
    [CreateAssetMenu(fileName = nameof(AvailableResources), menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Available Resources", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class AvailableResources : ScriptableObject
    {
        #region Properties and Fields

        public int CurrentResources { get; private set; }

        [SerializeField] private IntEvent resourcesChangedEvent;

        #endregion

        #region Resource Management

        public void SetResources(int quantity)
        {
            ModifyResources(quantity - CurrentResources);
        }

        public void AddResources(int quantity)
        {
            ModifyResources(quantity);
        }

        private void ModifyResources(int quantity)
        {
            CurrentResources += quantity;
            UnityEngine.Debug.Assert(CurrentResources >= 0, $"Now in negative resources ({CurrentResources}).");
            resourcesChangedEvent.Invoke(CurrentResources);
        }

        #endregion
    }
}