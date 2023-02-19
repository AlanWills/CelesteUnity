using Celeste.Events;

namespace Celeste.FSM.Nodes.Events
{
    [CreateNodeMenu("Celeste/Events/Raisers/Generic Event Raiser")]
    public class GenericEventRaiserNode : FSMNode
    {
        #region Properties and Fields

        public GenericEvent toRaise;
        public GenericEventArgs arguments;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            toRaise.Invoke(arguments);
        }

        #endregion
    }
}
