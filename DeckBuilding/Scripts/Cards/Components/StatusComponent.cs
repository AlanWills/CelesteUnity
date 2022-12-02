using Celeste.Components;
using Celeste.DeckBuilding.Components;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.DeckBuilding.Cards
{
    [Serializable, Flags]
    public enum CardStatus
    {
        None = 0,
        CannotPlay = 1,
        CannotDiscard = 2,
        RemovedFromDeckWhenPlayed = 4,
    }

    [DisplayName("Status")]
    public class StatusComponent : CardComponent
    {
        #region Save Data

        [Serializable]
        public class StatusComponentData : ComponentData
        {
            public CardStatus CardStatus;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private CardStatus initialCardStatus = CardStatus.None;

        #endregion

        public override ComponentData CreateData()
        {
            StatusComponentData statusData = new StatusComponentData();
            statusData.CardStatus = initialCardStatus;

            return statusData;
        }

        public bool HasCardStatus(Instance instance, CardStatus cardStatus)
        {
            StatusComponentData statusData = instance.data as StatusComponentData;
            return (statusData.CardStatus & cardStatus) == cardStatus;
        }

        public CardStatus GetCardStatus(Instance instance)
        {
            StatusComponentData statusData = instance.data as StatusComponentData;
            return statusData.CardStatus;
        }
    }
}