using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "New CameraReference", menuName = "Celeste/Parameters/Viewport/Camera Reference")]
    public class CameraReference : ParameterReference<Camera, CameraValue, CameraReference>
    {
    }
}
