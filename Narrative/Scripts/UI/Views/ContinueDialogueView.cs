using Celeste.FSM;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Continue Dialogue View")]
    public class ContinueDialogueView : NarrativeView
    {
        #region Narrative View

        public override bool IsValidForNode(FSMNode fsmNode)
        {
            return fsmNode is IDialogueNode && fsmNode.GetConnectedNodeFromDefaultOutput() != null;
        }

        public override void OnNodeEnter(FSMNode fsmNode) { }
        public override void OnNodeUpdate(FSMNode fsmNode) { }
        public override void OnNodeExit(FSMNode fsmNode) { }

        #endregion
    }
}
