using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Events;
using Celeste.DeckBuilding.Extensions;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeste.DeckBuilding.UI
{
    [AddComponentMenu("Celeste/Deck Building/UI/Ready UI Controller")]
    public class ReadyUIController : MonoBehaviour
    {
        #region Properties and Fields
        
        [Flags]
        public enum ReadyState
        {
            None = 0,
            Ready = 1,
            MouseOver = 2,
            ValidTarget = 4
        }

        private ReadyState state = ReadyState.None;
        private ReadyState State
        {
            get { return state; }
            set
            {
                state = value;
                UpdateUI();
            }
        }

        public bool Ready
        {
            set
            {
                if (value)
                {
                    State |= ReadyState.Ready;
                }
                else
                {
                    State &= ~ReadyState.Ready;
                }
            }
        }

        public bool MouseOver
        {
            set
            {
                if (value)
                {
                    State |= ReadyState.MouseOver;
                }
                else
                {
                    State &= ~ReadyState.MouseOver;
                }
            }
        }

        public bool ValidTarget
        {
            set
            {
                if (value)
                {
                    State |= ReadyState.ValidTarget;
                }
                else
                {
                    State &= ~ReadyState.ValidTarget;
                }
            }
        }

        [SerializeField] private GameObject readyUI;
        [SerializeField] private FX.Outline actorOutline;
        [SerializeField] private Color mouseOverColour;
        [SerializeField] private Color readyColour;
        [SerializeField] private Color validColour;

        private CardRuntime card;

        #endregion

        public void Hookup(CardRuntime card)
        {
            this.card = card;

            State = ReadyState.None;

            if (card.SupportsCombat())
            {
                card.AddOnReadyChangedCallback(OnReadyChanged);
                Ready = card.IsReady();
            }
        }

        private void OnDisable()
        {
            if (card.SupportsCombat())
            {
                card.RemoveOnReadyChangedCallback(OnReadyChanged);
            }
        }

        private void UpdateUI()
        {
            if ((state & ReadyState.MouseOver) == ReadyState.MouseOver)
            {
                bool isValidTarget = (state & ReadyState.ValidTarget) == ReadyState.ValidTarget;
                actorOutline.Colour = isValidTarget ? validColour : mouseOverColour;
                readyUI.SetActive(true);
            }
            else if ((state & ReadyState.Ready) == ReadyState.Ready)
            {
                actorOutline.Colour = readyColour;
                readyUI.SetActive(true);
            }
            else
            {
                readyUI.SetActive(false);
            }
        }

        #region Callbacks

        private void OnReadyChanged(bool isReady)
        {
            Ready = isReady;
        }

        #endregion
    }
}