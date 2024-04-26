using Celeste.Parameters;
using System;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Values/Float Comparer")]
    [NodeWidth(300)]
    public class FloatComparerNode : ValueComparerNode<float, FloatValue, FloatReference>
    {
    }
}
