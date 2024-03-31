using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Values/Vector3Int Comparer")]
    [NodeWidth(300)]
    public class Vector3IntComparerNode : ValueComparerNode<Vector3Int, Vector3IntValue, Vector3IntReference>
    {
    }
}
