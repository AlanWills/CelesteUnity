using Celeste.FSM;
using UnityEditor;
using UnityEngine;
using XNode.Attributes;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = "NarrativeGraph", menuName = "Celeste/Narrative/Narrative Graph")]
    public class NarrativeGraph : FSMGraph
    {
        [NodeGraphShortcut(KeyCode.T, EventModifiers.Shift)]
        public void ConnectSelectedNodes()
        {
            XNode.Node fromNode, toNode;
            var objects = Selection.objects;

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
        }
    }
}