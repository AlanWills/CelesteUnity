using Celeste.Parameters;
using System;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Values/Bool Comparer")]
    [NodeWidth(300)]
    public class BoolComparerNode : ValueComparerNode<bool, BoolValue, BoolReference>
    {
    }
}
