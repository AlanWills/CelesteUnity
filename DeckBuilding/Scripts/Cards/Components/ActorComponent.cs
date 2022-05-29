using Celeste.Components;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Actor")]
    public class ActorComponent : Components.Component
    {
        #region Save Data

        public class ActorComponentData : ComponentData
        {
            public bool IsOnStage;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private bool startsOnStage = false;
        [SerializeField] private Sprite sprite;

        #endregion

        public override ComponentData CreateData()
        {
            ActorComponentData actorData = new ActorComponentData();
            actorData.IsOnStage = startsOnStage;

            return actorData;
        }

        public bool IsOnStage(Instance instance)
        {
            ActorComponentData actorData = instance.data as ActorComponentData;
            return actorData.IsOnStage;
        }

        public bool SetOnStage(Instance instance, bool isOnStage)
        {
            ActorComponentData actorData = instance.data as ActorComponentData;
            return actorData.IsOnStage = isOnStage;
        }

        public Sprite GetSprite(Instance instance)
        {
            return sprite;
        }
    }
}