using Celeste.FSM;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Tap Receiver View")]
    public class TapReceiverView : NarrativeView, IPointerClickHandler
    {
        #region Properties and Fields

        private IPointerClickHandler clickHandlerNode;

        #endregion

        #region Narrative View

        public override bool IsValidForNode(FSMNode fsmNode)
        {
            return fsmNode is IPointerClickHandler;
        }

        public override void OnNodeEnter(FSMNode fsmNode)
        {
            clickHandlerNode = fsmNode as IPointerClickHandler;
        }

        public override void OnNodeUpdate(FSMNode fsmNode)
        {
        }

        public override void OnNodeExit(FSMNode fsmNode)
        {
            clickHandlerNode = null;
        }

        #endregion

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            // For some reason, after upgrading to the new input system I have to use pointerPress
            // instead of pointerClick.  It seems to work, but I don't know what's going on
            if (pointerEventData.pointerPress == gameObject && clickHandlerNode != null)
            {
                clickHandlerNode.OnPointerClick(pointerEventData);
            }
        }
    }
}
