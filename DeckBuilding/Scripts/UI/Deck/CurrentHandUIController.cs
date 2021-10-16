using Celeste.DeckBuilding.Cards;
using Celeste.Memory;
using Celeste.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Current Hand UI Controller")]
    public class CurrentHandUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObjectAllocator cardsAllocator;

        private List<CardUIController> cardControllers = new List<CardUIController>();

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGetInChildren(ref cardsAllocator);
        }

        #endregion

        public void Hookup(CurrentHand currentHand)
        {
            cardControllers.Clear();
            for (int i = 0, n = currentHand.NumCards; i < n; ++i)
            {
                AllocateCardController(currentHand.GetCard(i));
            }
        }

        private void AllocateCardController(CardRuntime card)
        {
            if (cardsAllocator.CanAllocate(1))
            {
                GameObject cardControllerGameObject = cardsAllocator.Allocate();
                CardUIController cardController = cardControllerGameObject.GetComponent<CardUIController>();
                cardController.Hookup(card);
                cardController.transform.SetSiblingIndex(0);
                cardControllerGameObject.SetActive(true);
                cardControllers.Add(cardController);
            }
            else
            {
                UnityEngine.Debug.LogAssertion("Could not allocate card controller.");
            }
        }

        private void DeallocateCardController(CardRuntime card)
        {
            int cardControllerIndex = cardControllers.FindIndex(x => x.IsForCard(card));
            if (cardControllerIndex >= 0)
            {
                CardUIController cardController = cardControllers[cardControllerIndex];
                cardsAllocator.Deallocate(cardController.gameObject);
                cardControllers.RemoveAt(cardControllerIndex);
            }
        }

        #region Callbacks

        public void OnCardAdded(CardRuntime card)
        {
            AllocateCardController(card);
        }

        public void OnCardRemoved(CardRuntime card)
        {
            DeallocateCardController(card);
        }

        #endregion
    }
}