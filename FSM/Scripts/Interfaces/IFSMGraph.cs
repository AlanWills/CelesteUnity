using System.Collections.Generic;

namespace Celeste.FSM
{
    public interface IFSMGraph
    {
        string name { get; }
        IFSMGraph ParentFSMGraph { get; }

        FSMNode StartNode { get; }
        IEnumerable<FSMNode> Nodes { get; }

        FSMNode FindNode(string nodeGuid);
    }
}