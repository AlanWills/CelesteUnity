using Celeste.BoardGame.Runtime;
using Celeste.Components;
using Celeste.Events;
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

        #endregion

        public void Hookup(BoardGameObjectRuntime runtime)
        {
            if (runtime.TryFindComponent(out token))
            {
                UpdateUI();

                token.iFace.AddIsFaceUpChangedListener(token.instance, OnIsFaceUpChanged);
            }
        }

        public void Shutdown()
        {
            if (token.IsValid)
            {
                token.iFace.RemoveIsFaceUpChangedListener(token.instance, OnIsFaceUpChanged);
                token.MakeNull();
            }
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

        #endregion
    }
}
