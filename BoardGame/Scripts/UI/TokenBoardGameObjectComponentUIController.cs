using Celeste.BoardGame.Interfaces;
using Celeste.BoardGame.Runtime;
using Celeste.Components;
using Celeste.Events;
using Celeste.Input;
using Celeste.UI;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Celeste.BoardGame.UI
{
    [AddComponentMenu("Celeste/Board Game/UI/Token UI Controller")]
    public class TokenBoardGameObjectComponentUIController : MonoBehaviour, IBoardGameObjectComponentUIController
    {
        #region Properties and Fields

        [Header("UI Elements")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        private InterfaceHandle<IBoardGameObjectToken> token;
        private InterfaceHandle<IBoardGameObjectTooltip> tooltip;

        #endregion

        public void Hookup(BoardGameObjectRuntime runtime)
        {
            if (runtime.TryFindComponent(out token))
            {
                UpdateUI();

                token.iFace.AddIsFaceUpChangedCallback(token.instance, OnIsFaceUpChanged);
            }

            runtime.TryFindComponent(out tooltip);
        }

        public void Shutdown()
        {
            if (token.IsValid)
            {
                token.iFace.RemoveIsFaceUpChangedCallback(token.instance, OnIsFaceUpChanged);
                token.MakeNull();
            }

            tooltip.MakeNull();
        }

        public void Flip()
        {
            if (token.IsValid)
            {
                token.iFace.Flip(token.instance);
            }
        }

        private void UpdateUI()
        {
            spriteRenderer.sprite = token.iFace.GetSprite(token.instance);
        }

        #region Callbacks

        private void OnIsFaceUpChanged(ValueChangedArgs<bool> valueChangedArgs)
        {
            UpdateUI();
        }

        public void OnMouseEnterToken(InputState inputState)
        {
            if (tooltip.IsValid)
            {
                tooltip.iFace.ShowTooltip(tooltip.instance, transform.position, true);
            }
        }

        public void OnMouseExitToken()
        {
            if (tooltip.IsValid)
            {
                tooltip.iFace.HideTooltip(tooltip.instance);
            }
        }

        #endregion
    }
}
