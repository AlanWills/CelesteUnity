using Celeste.DeckBuilding.Match;
using Celeste.FSM;
using Celeste.FSM.Nodes.Loading;
using Celeste.Parameters;
using Celeste.Scene.Events;
using System;

namespace Celeste.DeckBuilding.Nodes.Loading
{
    [Serializable]
    [CreateNodeMenu("Celeste/Deck Building/Loading/Load Deck Match")]
    public class LoadDeckMatchNode : LoadContextNode
    {
        #region Properties and Fields

        private bool MatchInProgress
        {
            get { return deckMatchInProgress == null || deckMatchInProgress.Value; }
        }

        public DeckMatch deckMatch;
        public BoolValue deckMatchInProgress;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            if (MatchInProgress)
            {
                // Load the context if we still have a match left to do
                base.OnEnter();
            }
        }

        protected override FSMNode OnUpdate()
        {
            // Progress if we have finished the match (aka we are returning from another FSM)
            return !MatchInProgress ? GetConnectedNode(DEFAULT_OUTPUT_PORT_NAME) : base.OnUpdate();
        }

        #endregion

        protected override Context CreateContext()
        {
            return deckMatch.CreateContext();
        }
    }
}
