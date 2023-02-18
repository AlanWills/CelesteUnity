using System.Collections.Generic;

namespace Celeste.FSM
{
    public interface IFSMGraph
    {
        string name { get; }
        IFSMGraph ParentFSMGraph { get; }

        FSMNode StartNode { get; }
        FSMNode FinishNode { get; }
        IEnumerable<FSMNode> Nodes { get; }

        FSMNode FindNode(string nodeGuid);
    }
}