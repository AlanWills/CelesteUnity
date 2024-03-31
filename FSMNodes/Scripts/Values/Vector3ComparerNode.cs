using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Values/Vector3 Comparer")]
    [NodeWidth(300)]
    public class Vector3ComparerNode : ValueComparerNode<Vector3, Vector3Value, Vector3Reference>
    {
    }
}
