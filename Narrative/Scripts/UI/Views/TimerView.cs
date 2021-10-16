using Celeste.FSM;
using Celeste.Narrative.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Narrative.UI
{
    [AddComponentMenu("Celeste/Narrative/UI/Timer View")]
    public class TimerView : NarrativeView
    {
        #region Properties and Fields

        [SerializeField] private Image timerFill;

        #endregion

        #region Narrative View

        public override bool IsValidForNode(FSMNode fsmNode)
        {
            return fsmNode is ITimedNode;
        }

        public override void OnNodeEnter(FSMNode fsmNode)
        {
            timerFill.fillAmount = 0;
        }

        public override void OnNodeUpdate(FSMNode fsmNode) 
        {
            ITimedNode timedNode = fsmNode as ITimedNode;
            timerFill.fillAmount = timedNode.ElapsedTime / timedNode.AllowedTime;
        }

        public override void OnNodeExit(FSMNode fsmNode) { }

        #endregion
    }
}
