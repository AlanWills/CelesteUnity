using Celeste.FSM;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Manual Continue View")]
    public class ManualContinueView : NarrativeView
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

        #region Callbacks

        public void Continue()
        {
            inputReceiverNode?.OnContinueInputReceived();
        }

        #endregion
    }
}
