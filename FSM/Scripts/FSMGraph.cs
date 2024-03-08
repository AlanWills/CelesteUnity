using Celeste.Objects;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Celeste.FSM
{
    [CreateAssetMenu(fileName = "FSMGraph", menuName = CelesteMenuItemConstants.FSM_MENU_ITEM + "FSM Graph", order = CelesteMenuItemConstants.FSM_MENU_ITEM_PRIORITY)]
    public class FSMGraph : NodeGraph, IParameterContainer, IFSMGraph
    {
        #region Properties and Fields

        public IFSMGraph ParentFSMGraph
        {
            get => parentFSMGraph;
            set => parentFSMGraph = value;
        }

        FSMNode IFSMGraph.StartNode => startNode;
        FSMNode IFSMGraph.FinishNode => finishNode;
        IEnumerable<FSMNode> IFSMGraph.Nodes
        {
            get
            {
                foreach (var node in nodes)
                {
                    yield return node as FSMNode;
                }
            }
        }

        public ILinearRuntime Runtime { get; set; }

        public FSMNode startNode;
        public FSMNode finishNode;

#if UNITY_EDITOR
        [SerializeField]
        private List<ScriptableObject> parameters = new List<ScriptableObject>();
#endif

        [NonSerialized] private IFSMGraph parentFSMGraph;

        #endregion

        public override NodeGraph Copy()
        {
            FSMGraph graph = base.Copy() as FSMGraph;

            for (int i = 0, n = graph.nodes.Count; i < n; i++)
            {
                (graph.nodes[i] as FSMNode).CopyInGraph(nodes[i] as FSMNode);
            }

            graph.startNode = graph.FindNode(startNode.Guid);
            graph.finishNode = graph.FindNode(finishNode.Guid);

            return graph;
        }

        #region Node Utility Methods

        public override Node AddNode(Type type)
        {
            FSMNode node = base.AddNode(type) as FSMNode;
            startNode = startNode == null ? node : startNode;
            node.AddToGraph();

            return node;
        }

        public override void RemoveNode(Node node)
        {
            FSMNode fsmNode = node as FSMNode;
            startNode = startNode == fsmNode ? null : startNode;
            fsmNode.RemoveFromGraph();

            base.RemoveNode(node);
        }

        public override Node CopyNode(Node original)
        {
            FSMNode copy = base.CopyNode(original) as FSMNode;
            copy.CopyInGraph(original as FSMNode);

            return copy;
        }

        public FSMNode FindNode(Predicate<FSMNode> predicate)
        {
            return nodes.Find(x => x is FSMNode && predicate(x as FSMNode)) as FSMNode;
        }

        public FSMNode FindNode(string nodeGuid)
        {
            return nodes.Find(x => x is FSMNode && string.CompareOrdinal((x as FSMNode).Guid, nodeGuid) == 0) as FSMNode;
        }

        public void RemoveAllNodes()
        {
            nodes.Clear();
            startNode = null;
            finishNode = null;

#if UNITY_EDITOR
            var objects = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(UnityEditor.AssetDatabase.GetAssetPath(this));
            Debug.Log($"Objects before: {objects.Length}");

            foreach (var obj in objects)
            {
                if (obj != this)
                {
                    DestroyImmediate(obj, true);
                }
            }

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();

            objects = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(UnityEditor.AssetDatabase.GetAssetPath(this));
            Debug.Log($"Objects after: {objects.Length}");
#endif
        }

#endregion

        #region Parameter Utility Methods

        public T CreateParameter<T>(string name) where T : ScriptableObject
        {
            T parameter = ScriptableObject.CreateInstance<T>();
            parameter.name = name;
            parameter.hideFlags = HideFlags.HideInHierarchy;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                parameters.Add(parameter);
                UnityEditor.AssetDatabase.AddObjectToAsset(parameter, this);
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif

            return parameter;
        }

        public T CreateParameter<T>(T original) where T : ScriptableObject, ICopyable<T>
        {
            T parameter = CreateParameter<T>(original.name);
            parameter.CopyFrom(original);

            return parameter;
        }

        public void RemoveAsset(ScriptableObject asset)
        {
            if (asset != null)
            {
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.RemoveObjectFromAsset(asset);
                parameters.Remove(asset);
#endif
                ScriptableObject.DestroyImmediate(asset);
            }
        }

        #endregion
    }
}
