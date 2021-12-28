using Celeste.Events;
using System;

namespace Celeste.FSM.Nodes.Events
{
    [CreateNodeMenu("Celeste/Events/Raisers/Event Raiser")]
    public class EventRaiserNode : FSMNode
    {
        #region Properties and Fields

        public Event toRaise;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            toRaise.Invoke();
        }

        #endregion
    }
}
