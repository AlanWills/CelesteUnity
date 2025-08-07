using System;
using System.ComponentModel;
using Celeste.FSM;
using UnityEngine;

namespace Celeste.Narrative.Requests
{
    [DisplayName("Animate Background Node")]
    [CreateNodeMenu("Celeste/Narrative/Animate Background")]
    public class AnimateBackgroundNode : NarrativeNode
    {
        #region Properties and Fields
        
        [SerializeField] private AnimateBackgroundRequest animateBackgroundRequest;
        [SerializeField] private AnimateBackgroundRequestArgs args;

        [NonSerialized] private bool responseReceived = false;

        #endregion
        
        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();
            
            responseReceived = false;
            animateBackgroundRequest.Raise(args, () => responseReceived = true, (code, message) =>
            {
                UnityEngine.Debug.LogError($"Failed to animate background in node {name} in graph {graph.name}.  Code: {code}, Message: {message}");
            });
        }

        protected override FSMNode OnUpdate()
        {
            return responseReceived ? GetConnectedNodeFromDefaultOutput() : this;
        }

        protected override void OnExit()
        {
            base.OnExit();
            
            responseReceived = false;
        }
        
        #endregion

        #region Copy
        
        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);
            
            AnimateBackgroundNode animateBackgroundNode = original as AnimateBackgroundNode;
            animateBackgroundRequest = animateBackgroundNode.animateBackgroundRequest;
            args = animateBackgroundNode.args;
        }
        
        #endregion
    }
}