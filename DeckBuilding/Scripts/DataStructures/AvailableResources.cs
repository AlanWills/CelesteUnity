using Celeste.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding
{
    [AddComponentMenu("Celeste/Deck Building/Available Resources")]
    public class AvailableResources : MonoBehaviour
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