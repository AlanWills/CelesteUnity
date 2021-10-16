using Celeste.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.FSM.Nodes.Testing
{
    [Serializable]
    [CreateNodeMenu("Celeste/Testing/Finish Integration Test")]
    public class FinishIntegrationTestNode : FSMNode
    {
        #region Properties and Fields

        public StringEvent testResult;

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            testResult.Invoke(graph.name);
        }

        #endregion
    }
}
