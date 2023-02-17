using Celeste.Logic;
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
    public class IfCondition
    {
        public string Name;
        public Condition Condition;
        public bool UseArgument;

        public IfCondition(string name, Condition condition)
        {
            Name = name;
            Condition = condition;
        }
    }

    [Serializable]
    [CreateNodeMenu("Celeste/Logic/If")]
    [NodeTint(0.0f, 1, 1)]
    public class IfNode : FSMNode
    {
        #region Properties and Fields

        [Input] public object inArgument;
        [Output] public object outArgument;

        public uint NumConditions => (uint)conditions.Count;

        [SerializeField] private List<IfCondition> conditions = new List<IfCondition>();

        #endregion

        #region Add/Remove/Copy

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            IfNode originalIfNode = original as IfNode;
            originalIfNode.conditions.Clear();

            for (uint i = 0; i < originalIfNode.NumConditions; ++i)
            {
                IfCondition originalCondition = originalIfNode.GetCondition(i);
                AddCondition(originalCondition.Name, originalCondition.Condition);
            }
        }

        protected override void OnRemoveFromGraph()
        {
            base.OnRemoveFromGraph();

            for (uint i = NumConditions; i > 0; --i)
            {
                RemoveCondition(i - 1);
            }
        }

        #endregion

        #region Condition Methods

        public void AddCondition(string conditionName)
        {
            AddCondition(conditionName, null);
        }

        public void AddCondition(string conditionName, Condition condition)
        {
            conditions.Add(new IfCondition(conditionName, condition));

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif

            AddOutputPort(conditionName);
        }

        public void RemoveCondition(uint conditionIndex)
        {
#if INDEX_CHECKS
            if (conditionIndex >= NumConditions)
            {
                return;
            }
#endif

            IfCondition ifCondition = GetCondition(conditionIndex);

            if (HasPort(ifCondition.Name))
            {
                RemoveDynamicPort(ifCondition.Name);
            }

            conditions.RemoveAt((int)conditionIndex);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public IfCondition GetCondition(uint conditionIndex)
        {
            return conditionIndex < NumConditions ? conditions[(int)conditionIndex] : null;
        }

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            return outArgument;
        }

        #endregion

        #region FSM Runtime

        protected override FSMNode OnUpdate()
        {
            object _argument = GetInputValue(nameof(inArgument), inArgument);
            outArgument = _argument;

            foreach (var condition in conditions)
            {
                if (condition.UseArgument)
                {
                    condition.Condition.SetVariable(_argument);
                }

                if (condition.Condition.IsMet)
                {
                    Debug.LogFormat("If Node condition {0} passed with argument {1} in FSM {2}", condition.Name, _argument != null ? _argument : "null", graph.name);
                    return GetConnectedNode(condition.Name);
                }
            }

            // Fall back to else port
            Debug.LogFormat("If Node no conditions passed with argument {0} in FSM {1}", _argument != null ? _argument : "null", graph.name);
            return base.OnUpdate();
        }

        #endregion
    }
}
