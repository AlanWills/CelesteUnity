using Celeste.Parameters;
using System;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Values/Int Comparer")]
    [NodeWidth(300)]
    public class IntComparerNode : ValueComparerNode<int, IntValue, IntReference>
    {
    }
}
