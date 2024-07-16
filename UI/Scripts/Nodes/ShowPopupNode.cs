using Celeste.Events;
using Celeste.FSM;

namespace Celeste.UI.Nodes
{
    [CreateNodeMenu("Celeste/UI/Show Popup")]
    public class ShowPopupNode : FSMNode
    {
        #region Properties and Fields

        public ShowPopupEvent toRaise;
        public ShowPopupArgs arguments;

        [Input] public object extraContext;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            toRaise.Invoke(arguments);
        }

        #endregion
    }
}