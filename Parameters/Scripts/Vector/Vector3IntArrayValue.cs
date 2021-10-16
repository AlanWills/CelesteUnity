using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "Vector3IntArrayValue", menuName = "Celeste/Parameters/Vector/Vector3Int Array Value")]
    public class Vector3IntArrayValue : ParameterValue<List<Vector3Int>>
    {
    }
}
