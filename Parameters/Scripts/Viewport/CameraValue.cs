using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "New CameraValue", menuName = "Celeste/Parameters/Viewport/Camera Value")]
    public class CameraValue : ParameterValue<Camera>
    {
    }
}
