using Celeste.FSM;
using Celeste.Input;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Tap To Continue View")]
    public class TapToContinueView : NarrativeView, IInputHandler
    {
        #region Properties and Fields

        private IInputReceiverNode inputReceiverNode;

        #endregion

        #region Narrative View

        public override bool IsValidForNode(FSMNode fsmNode)
        {
            return fsmNode is IInputReceiverNode;
        }

        public override void OnNodeEnter(FSMNode fsmNode)
        {
            inputReceiverNode = fsmNode as IInputReceiverNode;
        }

        public override void OnNodeUpdate(FSMNode fsmNode) { }

        public override void OnNodeExit(FSMNode fsmNode)
        {
            inputReceiverNode = null;
        }

        #endregion

        #region IInputHandler

        void IInputHandler.OnPointerEnter(InputState inputState)
        {
        }

        void IInputHandler.OnPointerOver(InputState inputState)
        {
        }

        void IInputHandler.OnPointerExit(InputState inputState)
        {
        }

        void IInputHandler.OnPointerFirstDown(InputState inputState)
        {
        }

        void IInputHandler.OnPointerDown(InputState inputState)
        {
        }

        void IInputHandler.OnPointerFirstUp(InputState inputState)
        {
            inputReceiverNode?.OnContinueInputReceived();
        }

        #endregion
    }
}
