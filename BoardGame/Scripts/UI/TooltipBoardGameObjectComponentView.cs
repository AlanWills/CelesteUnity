using Celeste.BoardGame.Interfaces;
using Celeste.BoardGame.Runtime;
using Celeste.Components;
using Celeste.Events;
using Celeste.Input;
using UnityEngine;

namespace Celeste.BoardGame.UI
{
    [AddComponentMenu("Celeste/Board Game/UI/Tooltip UI Controller")]
    public class TooltipBoardGameObjectComponentView : MonoBehaviour, IBoardGameObjectComponentView
    {
        #region Properties and Fields

        private InterfaceHandle<IBoardGameObjectTooltip> tooltip;

        #endregion

        public void Hookup(BoardGameObjectInstance instance)
        {
            if (!instance.TryFindComponent(out tooltip))
            {
                UnityEngine.Debug.LogAssertion($"Could not find component implementing {nameof(IBoardGameObjectTooltip)} interface on board game object {instance.Name}.");
            }
        }

        public void Shutdown()
        {
            tooltip.MakeNull();
        }

        #region Callbacks

        public void OnMouseEnterObject(InputState inputState)
        {
            if (tooltip.IsValid)
            {
                tooltip.iFace.ShowTooltip(tooltip.instance, transform.position, true);
            }
        }

        public void OnMouseExitObject()
        {
            if (tooltip.IsValid)
            {
                tooltip.iFace.HideTooltip(tooltip.instance);
            }
        }

        #endregion
    }
}
