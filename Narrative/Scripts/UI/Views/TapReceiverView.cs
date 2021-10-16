using Celeste.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (pointerEventData.pointerClick == gameObject && clickHandlerNode != null)
            {
                clickHandlerNode.OnPointerClick(pointerEventData);
            }
        }
    }
}
