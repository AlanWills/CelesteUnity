using Celeste.BoardGame.Interfaces;
using Celeste.BoardGame.Runtime;
using Celeste.Components;
using Celeste.Events;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.BoardGame.UI
{
    [AddComponentMenu("Celeste/Board Game/UI/Die 2D UI Controller")]
    public class Die2DBoardGameObjectComponentUIController : MonoBehaviour, IBoardGameObjectComponentUIController
    {
        #region Properties and Fields

        [Header("UI Elements")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        private InterfaceHandle<IBoardGameObjectDie2D> die2D;
        private InterfaceHandle<IBoardGameObjectTooltip> tooltip;

        #endregion

        public void Hookup(BoardGameObjectRuntime runtime)
        {
            if (runtime.TryFindComponent(out die2D))
            {
                UpdateUI();

                die2D.iFace.AddValueChangedListener(die2D.instance, OnValueChangedArgs);
            }

            runtime.TryFindComponent(out tooltip);
        }

        public void Shutdown()
        {
            if (die2D.IsValid)
            {
                die2D.iFace.RemoveValueChangedListener(die2D.instance, OnValueChangedArgs);
                die2D.MakeNull();
            }

            tooltip.MakeNull();
        }

        public void SetValue(int value)
        {
            if (die2D.IsValid)
            {
                die2D.iFace.SetValue(die2D.instance, value);
            }
        }

        private void UpdateUI()
        {
            spriteRenderer.sprite = die2D.iFace.GetSprite(die2D.instance);
        }

        #region Callbacks

        private void OnValueChangedArgs(ValueChangedArgs<int> args)
        {
            UpdateUI();
        }

        public void OnMouseEnterDie(Vector2 mousePosition)
        {
            if (tooltip.IsValid)
            {
                tooltip.iFace.ShowTooltip(tooltip.instance, transform.position, true);
            }
        }

        public void OnMouseExitDie()
        {
            if (tooltip.IsValid)
            {
                tooltip.iFace.HideTooltip(tooltip.instance);
            }
        }

        #endregion
    }
}
