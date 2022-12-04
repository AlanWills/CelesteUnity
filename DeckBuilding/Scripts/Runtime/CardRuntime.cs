using Celeste.Components;
using Celeste.Constants;
using Celeste.DataStructures;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Components;
using Celeste.DeckBuilding.Events;
using Celeste.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DeckBuilding
{
    [Serializable]
    public class CardRuntime : ComponentContainerRuntime<CardComponent>
    {
        #region Events

        public BoolUnityEvent OnFaceUpChanged { get; } = new BoolUnityEvent();
        public BoolUnityEvent OnCanPlayChanged { get; } = new BoolUnityEvent();
        public PlayCardSuccessUnityEvent OnPlayCardSuccess { get; } = new PlayCardSuccessUnityEvent();
        public PlayCardFailureUnityEvent OnPlayCardFailure { get; } = new PlayCardFailureUnityEvent();

        #endregion

        #region Properties and Fields

        public ID Owner { get; set; }

        public int CardGuid => card.Guid;
        public string CardName => card.name;
        public Sprite CardBack => card.CardBack;
        public Sprite CardFront => card.CardFront;

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

        [NonSerialized] private Card card;
        [NonSerialized] private bool faceUp = false;
        [NonSerialized] private bool canPlay;

        #endregion

        public CardRuntime(Card card)
        {
            this.card = card;

            InitComponents(card);
        }

        public bool IsForCard(Card card)
        {
            return this.card == card;
        }

        public bool TryPlay()
        {
            if (CanPlay)
            {
                OnPlayCardSuccess.Invoke(new PlayCardSuccessArgs() { cardRuntime = this });
                return true;
            }
            else
            {
                OnPlayCardFailure.Invoke(new PlayCardFailureArgs() { cardRuntime = this });
                return false;
            }
        }
    }
}