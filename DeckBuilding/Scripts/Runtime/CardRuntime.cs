using Celeste.Components;
using Celeste.Constants;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Components;
using Celeste.DeckBuilding.Decks;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using Celeste.Events;
using System;
using UnityEngine;

namespace Celeste.DeckBuilding
{
    [Serializable]
    public class CardRuntime : ComponentContainerRuntime<CardComponent>
    {
        #region Events

        public BoolUnityEvent OnFaceUpChanged { get; } = new BoolUnityEvent();
        public BoolUnityEvent OnCanPlayChanged { get; } = new BoolUnityEvent();
        public CardRuntimeUnityEvent OnPlayCardSuccess { get; } = new CardRuntimeUnityEvent();
        public CardRuntimeUnityEvent OnPlayCardFailure { get; } = new CardRuntimeUnityEvent();

        #endregion

        #region Properties and Fields

        public int CardGuid => card.Guid;
        public string CardName => card.name;
        public Sprite CardBack => card.CardBack;
        public Sprite CardFront => card.CardFront;
        public int DeckGuid => deck.Guid;

        public bool IsFaceUp
        {
            get { return faceUp; }
            set
            {
                if (faceUp != value)
                {
                    faceUp = value;
                    OnFaceUpChanged.Invoke(faceUp);
                }
            }
        }

        public bool CanPlay
        {
            get { return canPlay; }
            set
            {
                if (canPlay != value)
                {
                    canPlay = value;
                    OnCanPlayChanged.Invoke(canPlay);
                }
            }
        }

        [NonSerialized] private Deck deck;
        [NonSerialized] private Card card;
        [NonSerialized] private bool faceUp = false;
        [NonSerialized] private bool canPlay;

        #endregion

        public CardRuntime(Deck deck, Card card)
        {
            this.deck = deck;
            this.card = card;

            InitializeComponents(card);
        }

        public bool IsForCard(Card card)
        {
            return this.card == card;
        }

        public bool TryPlay()
        {
            if (CanPlay)
            {
                OnPlayCardSuccess.Invoke(this);

                if (this.IsRemovedFromDeckWhenPlayed())
                {
                    deck.AddCardToRemovedPile(this);
                }
                else
                {
                    deck.AddCardToDiscardPile(this);
                }

                return true;
            }
            else
            {
                OnPlayCardFailure.Invoke(this);
                return false;
            }
        }
    }
}