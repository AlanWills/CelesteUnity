using Celeste.DataStructures;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding
{
    [AddComponentMenu("Celeste/Deck Building/Removed Pile")]
    public class RemovedPile : MonoBehaviour, ICardStorage
    {
        #region Properties and Fields

        public int NumCards
        {
            get { return cards.Count; }
        }

        [SerializeField] private CardRuntimeEvent cardAddedEvent;
        [SerializeField] private Celeste.Events.Event clearedEvent;

        private List<CardRuntime> cards = new List<CardRuntime>();

        #endregion

        public void Clear()
        {
            if (cards.Count > 0)
            {
                cards.Clear();
                clearedEvent.Invoke();
            }
        }

        public CardRuntime GetCard(int index)
        {
            return cards.Get(index);
        }

        public void AddCard(CardRuntime card)
        {
            UnityEngine.Debug.Assert(card != null, $"Inputting null card into {nameof(RemovedPile.AddCard)}.");
            cards.Add(card);
            cardAddedEvent.Invoke(card);
        }
    }
}