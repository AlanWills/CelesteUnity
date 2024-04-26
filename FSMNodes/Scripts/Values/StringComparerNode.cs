using Celeste.Parameters;
using System;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Values/String Comparer")]
    [NodeWidth(300)]
    public class StringComparerNode : ValueComparerNode<string, StringValue, StringReference>
    {
    }
}
