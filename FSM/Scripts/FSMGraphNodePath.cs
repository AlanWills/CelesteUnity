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
                StringBuilder guidPathBuilder = new StringBuilder(64);
                StringBuilder readablePathBuilder = new StringBuilder(64);

                Node = fsmNode;

                do
                {
                    IFSMGraph parentGraph = fsmNode.FSMGraph;

                    guidPathBuilder.Append(fsmNode.Guid);
                    readablePathBuilder.Append(fsmNode.name);
                    readablePathBuilder.Append('.');
                    readablePathBuilder.Append(parentGraph.name);

                    // If our parent graph is actually contained within a linear runtime, we hop the boundary and continue
                    if (parentGraph.ParentFSMGraph is ILinearRuntime)
                    {
                        fsmNode = parentGraph.ParentFSMGraph as FSMNode;
                        guidPathBuilder.Append('.');
                        readablePathBuilder.Append('.');
                    }
                    else
                    {
                        fsmNode = null;
                    }    
                }
                while (fsmNode != null);

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