using Celeste.Logic;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Celeste.FSM.Nodes.Logic
{
    [Serializable]
    [CreateNodeMenu("Celeste/Logic/And")]
    [NodeTint(0.0f, 1, 1)]
    public class AndNode : FSMNode
    {
        #region Properties and Fields

        public BoolValue value1;
        public BoolValue value2;

        private const string TRUE_OUTPUT_PORT = "True";
        private const string FALSE_OUTPUT_PORT = "False";

        #endregion

        public AndNode()
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);
            AddOutputPort(TRUE_OUTPUT_PORT);
            AddOutputPort(FALSE_OUTPUT_PORT);
        }

        #region FSM Runtime

        protected override FSMNode OnUpdate()
        {
            BoolValue _value1 = GetInputValue(nameof(value1), value1);
            BoolValue _value2 = GetInputValue(nameof(value2), value2);

            return _value1.Value && _value2.Value ? GetConnectedNode(TRUE_OUTPUT_PORT) : GetConnectedNode(FALSE_OUTPUT_PORT);
        }

        #endregion
    }
}
