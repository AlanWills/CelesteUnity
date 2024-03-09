using Celeste.FSM;
using UnityEngine;
using XNode;
using XNode.Attributes;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = "NarrativeGraph", menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Narrative Graph", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class NarrativeGraph : FSMGraph, IProgressFSMGraph
    {
        #region Properties and Fields

        public FSMNode FinishNode
        {
            get => finishNode;
            set
            {
                if (finishNode != value)
                {
                    finishNode = value;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                }
            }
        }

        [SerializeField] private FSMNode finishNode;

        #endregion

        public override NodeGraph Copy()
        {
            NarrativeGraph narrativeGraph = base.Copy() as NarrativeGraph;
            narrativeGraph.FinishNode = narrativeGraph.FindNode(finishNode.Guid);

            return narrativeGraph;
        }

        [NodeGraphShortcut(KeyCode.T, EventModifiers.Shift)]
        public void ConnectSelectedNodes()
        {
#if UNITY_EDITOR
            XNode.Node fromNode, toNode;
            var objects = UnityEditor.Selection.objects;

            if (objects != null && objects.Length > 1)
            {
                fromNode = objects[0] as XNode.Node;

                for (int i = 1; i < objects.Length; ++i)
                {
                    toNode = objects[i] as XNode.Node;

                    var outputPort = fromNode.GetOutputPort(FSMNode.DEFAULT_OUTPUT_PORT_NAME);
                    var inputPort = toNode.GetInputPort(FSMNode.DEFAULT_INPUT_PORT_NAME);
                    outputPort.Connect(inputPort);

                    fromNode = toNode;
                }
            }
#endif
        }
    }
}