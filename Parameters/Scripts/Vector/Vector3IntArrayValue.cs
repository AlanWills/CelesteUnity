using Celeste.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(Vector3IntArrayValue), menuName = "Celeste/Parameters/Vector/Vector3Int Array Value")]
    public class Vector3IntArrayValue : ParameterValue<List<Vector3Int>, Vector3IntArrayValueChangedEvent>
    {
    }
}
