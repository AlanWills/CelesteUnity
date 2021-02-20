using Celeste.FSM.Nodes.Logic.Conditions;
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
    [CreateNodeMenu("Celeste/Logic/If")]
    [NodeTint(0.0f, 1, 1)]
    public class IfNode : FSMNode
    {
        #region Properties and Fields

        [Input]
        public object inArgument;

        [Output]
        public object outArgument;

        public uint NumConditions
        {
            get { return (uint)conditions.Count; }
        }

        [SerializeField]
        private List<Conditions.Condition> conditions = new List<Conditions.Condition>();

        #endregion

        #region Add/Remove/Copy

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            IfNode originalIfNode = original as IfNode;

            for (uint i = 0; i < originalIfNode.NumConditions; ++i)
            {
                Conditions.Condition originalCondition = originalIfNode.GetCondition(i);
                Conditions.Condition newCondition = AddCondition(originalCondition.name, originalCondition.GetType());
                newCondition.CopyFrom(originalCondition);
            }
        }

        protected override void OnRemoveFromGraph()
        {
            base.OnRemoveFromGraph();

            for (uint i = NumConditions; i > 0; --i)
            {
                RemoveCondition(GetCondition(i - 1));
            }
        }

        #endregion

        #region Condition Methods

        public Conditions.Condition AddCondition(string conditionName, Type conditionType)
        {
            Conditions.Condition valueCondition = ScriptableObject.CreateInstance(conditionType) as Conditions.Condition;
            valueCondition.name = conditionName;
            conditions.Add(valueCondition);

#if UNITY_EDITOR
            valueCondition.Init_EditorOnly(graph as FSMGraph);

            UnityEditor.AssetDatabase.AddObjectToAsset(valueCondition, graph);
            UnityEditor.EditorUtility.SetDirty(graph);
            UnityEditor.AssetDatabase.SaveAssets();
#endif

            AddOutputPort(conditionName);

            return valueCondition;
        }

        public T AddCondition<T>(string conditionName) where T : Conditions.Condition
        {
            return AddCondition(conditionName, typeof(T)) as T;
        }

        public void RemoveCondition(Conditions.Condition condition)
        {
            if (HasPort(condition.name))
            {
                RemoveDynamicPort(condition.name);
            }

            conditions.Remove(condition);

#if UNITY_EDITOR
            condition.Cleanup_EditorOnly(graph as FSMGraph);

            UnityEditor.AssetDatabase.RemoveObjectFromAsset(condition);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        public Conditions.Condition GetCondition(uint conditionIndex)
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

            foreach (Conditions.Condition condition in conditions)
            {
                if (condition.Check(_argument))
                {
                    Debug.LogFormat("If Node condition {0} passed with argument {1} in FSM {2}", condition.name, _argument != null ? _argument : "null", graph.name);
                    return GetConnectedNode(condition.name);
                }
            }

            // Fall back to else port
            Debug.LogFormat("If Node no conditions passed with argument {0} in FSM {1}", _argument != null ? _argument : "null", graph.name);
            return base.OnUpdate();
        }

        #endregion
    }
}
