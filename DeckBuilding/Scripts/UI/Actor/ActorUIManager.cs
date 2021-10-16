using Celeste.DataStructures;
using Celeste.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Actor UI Manager")]
    public class ActorUIManager : MonoBehaviour
    {
        #region Properties and Fields

        public int NumCardActorUIs
        {
            get { return actorUIControllers.Count; }
        }

        [SerializeField] private GameObjectAllocator cardActorUIAllocator;

        private List<ActorUIController> actorUIControllers = new List<ActorUIController>();

        #endregion

        public void Hookup(Stage stage)
        {
            for (int i = 0, n = stage.NumCards; i < n; ++i)
            {
                TryAddCardActorUI(stage.GetCard(i));
            }
        }

        public ActorUIController FindCardActorUI(Predicate<ActorUIController> predicate)
        {
            return actorUIControllers.Find(predicate);
        }

        public ActorUIController GetCardActorUI(int index)
        {
            return actorUIControllers.Get(index);
        }

        private void TryAddCardActorUI(CardRuntime card)
        {
            if (cardActorUIAllocator.CanAllocate(1))
            {
                GameObject cardActorUIGameObject = cardActorUIAllocator.Allocate();
                ActorUIController cardActorUIController = cardActorUIGameObject.GetComponent<ActorUIController>();
                UnityEngine.Debug.Assert(cardActorUIController != null, $"Could not find {nameof(ActorUIController)}.");
                cardActorUIController.Hookup(card);
                cardActorUIGameObject.SetActive(true);
                actorUIControllers.Add(cardActorUIController);
            }
            else
            {
                UnityEngine.Debug.LogAssertion($"Failed to allocate card actor for card {card.CardName}.");
            }
        }

        private void TryRemoveCardActorUI(CardRuntime card)
        {
            ActorUIController actorUIController = actorUIControllers.Find(x => x.Card == card);
            
            if (actorUIController != null)
            {
                actorUIControllers.Remove(actorUIController);
                cardActorUIAllocator.Deallocate(actorUIController.gameObject);
            }
            else
            {
                UnityEngine.Debug.LogAssertion($"Failed to find card actor UI for card {card.CardName}.");
            }
        }

        #region Callbacks

        public void OnActorAdded(CardRuntime card)
        {
            TryAddCardActorUI(card);
        }

        public void OnActorRemoved(CardRuntime card)
        {
            TryRemoveCardActorUI(card);
        }

        #endregion
    }
}