using Celeste.Parameters;
using System;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Values/UInt Comparer")]
    [NodeWidth(300)]
    public class UIntComparerNode : ValueComparerNode<uint, UIntValue, UIntReference>
    {
    }
}
