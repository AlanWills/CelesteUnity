using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "TransformReference", menuName = "Celeste/Parameters/Transform/Transform Reference")]
    public class TransformReference : ParameterReference<Transform, TransformValue, TransformReference>
    {
    }
}
