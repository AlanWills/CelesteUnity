using System;
using System.Text;

namespace Celeste.FSM
{
    [Serializable]
    public struct FSMGraphNodePath
    {
        #region Properties and Fields

        public FSMNode Node { get; private set; }
        public string Path { get; private set; }

        public static readonly FSMGraphNodePath EMPTY = new FSMGraphNodePath(null);

        #endregion

        public FSMGraphNodePath(IFSMGraph graph, string path)
        {
            Node = null;
            Path = path;

            string[] subPaths = Path.Split('.');

            for (int i = subPaths != null ? subPaths.Length : 0; i > 0; --i)
            {
                Node = graph.FindNode(subPaths[i - 1]);
                graph = Node as IFSMGraph;
            }
        }

        public FSMGraphNodePath(FSMNode fsmNode)
        {
            if (fsmNode != null)
            {
                Node = fsmNode;

                StringBuilder pathBuilder = new StringBuilder(32);
                IFSMGraph parentGraph = fsmNode.FSMGraph;

                while (fsmNode != null)
                {
                    pathBuilder.Append(fsmNode.Guid);
                    parentGraph = parentGraph.ParentFSMGraph;

                    if (parentGraph != null)
                    {
                        pathBuilder.Append('.');
                    }
                    else
                    {
                        break;
                    }
                }

                Path = pathBuilder.ToString();
            }
            else
            {
                Node = null;
                Path = string.Empty;
            }
        }
    }
}