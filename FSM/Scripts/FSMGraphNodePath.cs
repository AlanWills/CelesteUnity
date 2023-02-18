using System;
using System.Linq;
using System.Text;

namespace Celeste.FSM
{
    [Serializable]
    public struct FSMGraphNodePath
    {
        #region Properties and Fields

        public FSMNode Node { get; }
        public string GuidPath { get; }
        public string ReadablePath { get; }

        public static readonly FSMGraphNodePath EMPTY = new FSMGraphNodePath(null);

        #endregion

        public FSMGraphNodePath(IFSMGraph graph, string guidPath)
        {
            Node = null;
            GuidPath = guidPath;

            string[] subPaths = GuidPath.Split('.');
            StringBuilder readablePath = new StringBuilder(64);

            for (int i = subPaths != null ? subPaths.Length : 0; i > 0; --i)
            {
                Node = graph.FindNode(subPaths[i - 1]);
                graph = Node as IFSMGraph;

                readablePath.Append(Node.name);
                readablePath.Append(".");
            }

            readablePath.Append(Node.FSMGraph.name);
            ReadablePath = readablePath.ToString();
        }

        public FSMGraphNodePath(FSMNode fsmNode)
        {
            if (fsmNode != null)
            {
                Node = fsmNode;

                StringBuilder guidPathBuilder = new StringBuilder(64);
                StringBuilder readablePathBuilder = new StringBuilder(64);
                guidPathBuilder.Append(fsmNode.Guid);
                readablePathBuilder.Append(fsmNode.name);

                IFSMGraph parentGraph = fsmNode.FSMGraph;
                
                while (parentGraph != null)
                {
                    // TODO: How do we find the sub fsm node that our parent graph could be part of?
                    //guidPathBuilder.Append('.');
                    //guidPathBuilder.Append(parentGraph.Guid);
                    readablePathBuilder.Append('.');
                    readablePathBuilder.Append(parentGraph.name);
                    parentGraph = parentGraph.ParentFSMGraph;
                }

                GuidPath = guidPathBuilder.ToString();
                ReadablePath = readablePathBuilder.ToString();
            }
            else
            {
                Node = null;
                GuidPath = string.Empty;
                ReadablePath = "null";
            }
        }
    }
}